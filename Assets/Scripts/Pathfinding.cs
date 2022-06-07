using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int pos_x;
    public int pos_y;

    public float gValue; //시작점에서 해당 사각형까지의 거리
    public float hValue; //해당 사각형에서 도착지점까지의 거리
    public PathNode parentNode;

    public float fValue //총합값 -- 걸은값 + 걸어가야하는 값
    {
        get { return gValue + hValue; }
    }

    public PathNode(int xPos, int yPos)
    {
        pos_x = xPos;
        pos_y = yPos;
    }

    public void Clear()
    {
        gValue = 0f;
        hValue = 0f;
        parentNode = null;
    }
}

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    //A* 알고리즘

    Grid gridMap;
    PathNode[,] pathNodes; //2차원 배열, 좌표임

    void Start()
    {
        Init();
    }
    internal void Clear()
    {
        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.length; y++)
            {
                pathNodes[x, y].Clear();
            }
        }
    }

    private void Init()
    {
        if(gridMap == null) //그리드맵의 정보 유무 확인
        {
            gridMap = GetComponent<Grid>(); //없으면 가져온다.
        }
        pathNodes = new PathNode[gridMap.width, gridMap.length]; //새[x,y] 객체 생성

        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.length; y++)
            {
                pathNodes[x, y] = new PathNode(x, y);//각 노드마다 새 객체 배정해서 initialize
            }
        }
    }

    public void CalculateWalkableNodes(int startX, int startY, float range, ref List<PathNode> toHighlight)
    {
        PathNode startNode = pathNodes[startX, startY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closedList.Add(currentNode);


            List<PathNode> neighbourNodes = new List<PathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false)
                    {
                        continue;
                    }

                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }

                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    if (closedList.Contains(neighbourNodes[i]))
                    {
                        continue;
                    }
                    if (gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false)
                    {
                        continue;
                    }

                    float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                    if(movementCost >range)
                    {
                        continue;
                    }

                    if (openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                    {
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].parentNode = currentNode;

                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }
            }
        }

        if(toHighlight != null)
        {
            toHighlight.AddRange(closedList);
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //두 지점간에서의 PathNode상에서의 길을 찿아줌
        PathNode startNode = pathNodes[startX, startY]; //시작지점 저장
        PathNode endNode = pathNodes[endX, endY]; //도착지점 저장

        List<PathNode> openList = new List<PathNode>();//갈 지점 리스트
        List<PathNode> closedList = new List<PathNode>();//갔던 지점 리스트

        openList.Add(startNode);//시작지점 리스트에 저장

        while(openList.Count > 0) //도착할때까지 혹은 다른 수단이 없을 때까지 반복수행
        {
            PathNode currentNode = openList[0];//자신의 위치를 리스트 첫번째꺼에서 꺼내옴
            for (int i = 0; i < openList.Count; i++)
            {
                if(currentNode.fValue > openList[i].fValue)//주변 노드의 f값을 비교
                {
                    //낮으면 현재 노드로 지정 -- 갈 곳임, 데이터 상으로는 그쪽으로 진행함
                    //만약 주변에 더 낮은 f값이 나왔으면 간거 취소하고 낮은 쪽으로 진행
                    currentNode = openList[i];
                }
                //f값이 만약 같은 애가 있다 그러면 h값으로 비교해서 h값이 낮은 곳으로 감
                if(currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];//간다!
                }
            }
            //비교 다했으면 원래 있었던 곳을 리스트에서 폐기한다.
            openList.Remove(currentNode);
            //폐기된 애가 가는 리스트 -- 얘가 최단거리를 기억함
            //주기억장치에서 보조기억장치로 데이터 옮기는거라 생각하면 됨 -- 실제로 움직이는게 아니라 데이터 상으로만 움직이고 있으니깐
            closedList.Add(currentNode);

            //도착시
            if(currentNode == endNode)
            {
                return RetracePath(startNode, endNode);//길 정제
            }

            //주변 탐색 (3X3)
            List<PathNode> neighbourNodes = new List<PathNode>();//주변애들 넣을 새 리스트
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if(x == 0 && y == 0)//자신(정중앙)은 제외
                    {
                        continue;
                    }
                    //주변 애들이 설정된 좌표 내에 존재하는지
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false)
                    {
                        continue; 
                    }
                    //검사 후 리스트에 추가
                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
                //주변 애들 
                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    if (closedList.Contains(neighbourNodes[i]))//이미 갔던 곳인가? 그렇다면 스킵
                    {
                        continue;
                    }
                    if(gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false)//못가는 곳임? 그럼 스킵
                    {
                        continue;
                    }
                    //주변 애들까지의 거리 측정
                    float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);
                    //자기 g값보다 낮을 경우 또는 이미 갈 곳 리스트에 추가된 게 아니면
                    if(openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                    {
                        //지정해준다.
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);//h값도 계산해서 넣어주자
                        //이 노드에 도달하는 경로 노드에 부모 노드를 저장 -- 뭔 말인지 확실하게 이해는 가지 않는다 번역기 복붙
                        //아마 계산된 노드 중 경로가 될 노드가 저장되는 곳일듯 => 값이 젤 낮은 애
                        neighbourNodes[i].parentNode = currentNode;

                        //갈 곳 리스트에 추가 안됐냐?
                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            //고럼 추가해
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }

            }
        }

        return null; //길 못찿음. null 뱉을거야
    }

    private int CalculateDistance(PathNode currentNode, PathNode target)
    {
        //상대와의 거리를 x,y로 분해해 절대값으로 내줌
        int distX = Mathf.Abs(currentNode.pos_x - target.pos_x);
        int distY = Mathf.Abs(currentNode.pos_y - target.pos_y);

        if(distX > distY)//둘 다 값이 있으면? 대각선이 있다! 피타고라스의 정리를 사용하자!
        {
            //10 / cos45 = 14.xxx, 이걸 대각선 길이만큼 곱하면 비례관계에 따라 답이 나온다.
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        //parentNode를 이용해 retrace 함
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = endNode; //거슬러 올라가는거니 끝지점을 시작지점 및 현재지점으로

        while(currentNode != startNode)//도착할때까지
        {
            path.Add(currentNode);//길을 리스트에 하나씩 저장
            currentNode = currentNode.parentNode;//저장한 후 한단계 거슬러 올라간다
        }
        path.Reverse();//리스트를 거꾸로 돌려준다.

        return path;
    }

    public List<PathNode> TraceBackPath(int x, int y)
    {
        if (gridMap.CheckBoundry(x, y) == false)
        {
            return null;
        }
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = pathNodes[x, y];
        while(currentNode.parentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        return path;
    }

}
