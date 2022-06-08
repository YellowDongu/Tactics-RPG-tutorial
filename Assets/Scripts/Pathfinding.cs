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
    public PathNode parentNode;//이 노드에 오기 전에 있었던 노드, 노드 안에 노드가 저장되고 그 노드 안에 노드가 저장되고...

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
    //이 스크립트를 프로젝트 세팅에서 스크립트 엑시큐션 오더를 디폴트 위(시네머신유니버셜픽셀퍼펙트 위)로 올려야된다
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
                //계산값 초기화
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
        //캐릭터가 움직일 수 있는 칸을 계산해서 하이라이트로 칠하고 추가로 리스트에도 올림
        //이거 역시 A*알고리즘을 사용함
        //내가 만들때는 하이라이트만 필요없으니 나중에 빼버리거나 상황이 변하면 이 문장을 수정하자
        //이 메소드의 마지막 인수인 리스트는 계산된 노드들을 등록할 곳
        PathNode startNode = pathNodes[startX, startY];//캐릭터 현재 지점

        //A* 알고리즘 참고
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);//현재지점 저장
        //리스트 항목이 있을때
        while (openList.Count > 0)
        {
            //현재 노드를 꺼낸다. 그 노드는 출발지점이다. 현재 밟고 있음
            PathNode currentNode = openList[0];
            //현재 노드를 폐기한다
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //주변 노드 리스트 생성
            List<PathNode> neighbourNodes = new List<PathNode>();
            //3X3 주변칸 수색
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)//중앙칸 제거와 좌표가 맵 밖일때 건너뜀
                    {
                        continue;
                    }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false)
                    {
                        continue;
                    }
                    //심사 통과한 애들을 추가한다.
                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
                //주변칸 계산
                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    //이미 클로즈드리스트에 있음(이전에 간 곳)
                    if (closedList.Contains(neighbourNodes[i]))
                    {
                        continue;
                    }
                    //갈 수 있는 곳인지 확인
                    if (gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false)
                    {
                        continue;
                    }
                    //칸의 g값과 앞으로 갈 거리를 합침 = 시작부터 거기까지 도달하는데 드는 비용
                    float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                    //칸의 총 비용과 캐릭터 이동값 비교
                    if (movementCost >range)
                    {
                        continue;//이동 범위 밖임 = 코스트 부족 => 못가 히히
                    }
                    //등록된 애의 g값보다 낮을 경우 또는 or 갈 곳 리스트에 추가된 게 아니면
                    if (openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                    {
                        //등록해준다 -- 얘가 앞으로 갈 애
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].parentNode = currentNode;
                        //오픈 리스트에도 등록 안되있으면 등록해준다.
                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }
            }
        }
        //하이라이트 범위가 지정 안되있으면
        if(toHighlight != null)
        {
            //클로즈드 리스트에 넣은 애들 = 갈 수 있는 애들이니깐 통째로 넣어준다.
            //findpath에 이 구절과 인수 추가 하나면 외부 리스트에 계산결과를 넣을 수 있다!
            toHighlight.AddRange(closedList);
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //두 지점간에서의 PathNode상에서의 길을 찿아줌
        PathNode startNode = pathNodes[startX, startY]; //시작지점 저장
        PathNode endNode = pathNodes[endX, endY]; //도착지점 저장

        List<PathNode> openList = new List<PathNode>();//경로 분석을 위한 임시 저장소
        List<PathNode> closedList = new List<PathNode>();//처리 완료된 모든 노드들을 위한 임시 저장소

        openList.Add(startNode);//시작지점 리스트에 저장
        //도착할때까지 혹은 다른 수단이 없을 때까지 반복수행
        //나온 마지막 탐색 경로를 크로즈드 리스트에 넣고 더이상 갈곳이 없을 때 루프 탈출
        //탈출 조건 : 경로 없음 혹은 도착지점 도달
        while (openList.Count > 0) 
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
                        //현재 노드를 다음 노드의 부모 노드로 저장시킴
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
        //parentNode를 직접 받아서 사용하는 애
        //parentNode를 통해 저장된 경로들을 하나씩 꺼내서 리스트에 배치시키는 메소드
        //이렇게 저장하면 도착지점에서 시작지점으로 가는 길이 되어버리니 리스트 Reverse는 필수
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
        //x y 좌표를 받아 거기에 있는 노드의 parentNode를 받아 경로를 꺼내는 애
        //맵 밖의 좌표는 스킵
        if (gridMap.CheckBoundry(x, y) == false)
        {
            return null;
        }
        List<PathNode> path = new List<PathNode>();
        //받은 좌표의 노드 불러옴
        PathNode currentNode = pathNodes[x, y];
        //경로 노드(경로로 지정된 노드)가 있으면 없을때까지 시행
        while(currentNode.parentNode != null)
        {
            //현재 선택된 좌표를 리스트에 넣고
            path.Add(currentNode);
            currentNode = currentNode.parentNode;//거슬러 올라간다
        }
        return path;
    }

}
