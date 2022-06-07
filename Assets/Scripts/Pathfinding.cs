using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int pos_x;
    public int pos_y;

    public float gValue; //���������� �ش� �簢�������� �Ÿ�
    public float hValue; //�ش� �簢������ �������������� �Ÿ�
    public PathNode parentNode;

    public float fValue //���հ� -- ������ + �ɾ���ϴ� ��
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
    //A* �˰���

    Grid gridMap;
    PathNode[,] pathNodes; //2���� �迭, ��ǥ��

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
        if(gridMap == null) //�׸������ ���� ���� Ȯ��
        {
            gridMap = GetComponent<Grid>(); //������ �����´�.
        }
        pathNodes = new PathNode[gridMap.width, gridMap.length]; //��[x,y] ��ü ����

        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.length; y++)
            {
                pathNodes[x, y] = new PathNode(x, y);//�� ��帶�� �� ��ü �����ؼ� initialize
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
        //�� ������������ PathNode�󿡼��� ���� �O����
        PathNode startNode = pathNodes[startX, startY]; //�������� ����
        PathNode endNode = pathNodes[endX, endY]; //�������� ����

        List<PathNode> openList = new List<PathNode>();//�� ���� ����Ʈ
        List<PathNode> closedList = new List<PathNode>();//���� ���� ����Ʈ

        openList.Add(startNode);//�������� ����Ʈ�� ����

        while(openList.Count > 0) //�����Ҷ����� Ȥ�� �ٸ� ������ ���� ������ �ݺ�����
        {
            PathNode currentNode = openList[0];//�ڽ��� ��ġ�� ����Ʈ ù��°������ ������
            for (int i = 0; i < openList.Count; i++)
            {
                if(currentNode.fValue > openList[i].fValue)//�ֺ� ����� f���� ��
                {
                    //������ ���� ���� ���� -- �� ����, ������ �����δ� �������� ������
                    //���� �ֺ��� �� ���� f���� �������� ���� ����ϰ� ���� ������ ����
                    currentNode = openList[i];
                }
                //f���� ���� ���� �ְ� �ִ� �׷��� h������ ���ؼ� h���� ���� ������ ��
                if(currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];//����!
                }
            }
            //�� �������� ���� �־��� ���� ����Ʈ���� ����Ѵ�.
            openList.Remove(currentNode);
            //���� �ְ� ���� ����Ʈ -- �갡 �ִܰŸ��� �����
            //�ֱ����ġ���� ���������ġ�� ������ �ű�°Ŷ� �����ϸ� �� -- ������ �����̴°� �ƴ϶� ������ �����θ� �����̰� �����ϱ�
            closedList.Add(currentNode);

            //������
            if(currentNode == endNode)
            {
                return RetracePath(startNode, endNode);//�� ����
            }

            //�ֺ� Ž�� (3X3)
            List<PathNode> neighbourNodes = new List<PathNode>();//�ֺ��ֵ� ���� �� ����Ʈ
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if(x == 0 && y == 0)//�ڽ�(���߾�)�� ����
                    {
                        continue;
                    }
                    //�ֺ� �ֵ��� ������ ��ǥ ���� �����ϴ���
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false)
                    {
                        continue; 
                    }
                    //�˻� �� ����Ʈ�� �߰�
                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
                //�ֺ� �ֵ� 
                for (int i = 0; i < neighbourNodes.Count; i++)
                {
                    if (closedList.Contains(neighbourNodes[i]))//�̹� ���� ���ΰ�? �׷��ٸ� ��ŵ
                    {
                        continue;
                    }
                    if(gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false)//������ ����? �׷� ��ŵ
                    {
                        continue;
                    }
                    //�ֺ� �ֵ������ �Ÿ� ����
                    float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);
                    //�ڱ� g������ ���� ��� �Ǵ� �̹� �� �� ����Ʈ�� �߰��� �� �ƴϸ�
                    if(openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                    {
                        //�������ش�.
                        neighbourNodes[i].gValue = movementCost;
                        neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);//h���� ����ؼ� �־�����
                        //�� ��忡 �����ϴ� ��� ��忡 �θ� ��带 ���� -- �� ������ Ȯ���ϰ� ���ش� ���� �ʴ´� ������ ����
                        //�Ƹ� ���� ��� �� ��ΰ� �� ��尡 ����Ǵ� ���ϵ� => ���� �� ���� ��
                        neighbourNodes[i].parentNode = currentNode;

                        //�� �� ����Ʈ�� �߰� �ȵƳ�?
                        if (openList.Contains(neighbourNodes[i]) == false)
                        {
                            //�� �߰���
                            openList.Add(neighbourNodes[i]);
                        }
                    }
                }

            }
        }

        return null; //�� ���O��. null �����ž�
    }

    private int CalculateDistance(PathNode currentNode, PathNode target)
    {
        //������ �Ÿ��� x,y�� ������ ���밪���� ����
        int distX = Mathf.Abs(currentNode.pos_x - target.pos_x);
        int distY = Mathf.Abs(currentNode.pos_y - target.pos_y);

        if(distX > distY)//�� �� ���� ������? �밢���� �ִ�! ��Ÿ����� ������ �������!
        {
            //10 / cos45 = 14.xxx, �̰� �밢�� ���̸�ŭ ���ϸ� ��ʰ��迡 ���� ���� ���´�.
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        //parentNode�� �̿��� retrace ��
        List<PathNode> path = new List<PathNode>();

        PathNode currentNode = endNode; //�Ž��� �ö󰡴°Ŵ� �������� �������� �� ������������

        while(currentNode != startNode)//�����Ҷ�����
        {
            path.Add(currentNode);//���� ����Ʈ�� �ϳ��� ����
            currentNode = currentNode.parentNode;//������ �� �Ѵܰ� �Ž��� �ö󰣴�
        }
        path.Reverse();//����Ʈ�� �Ųٷ� �����ش�.

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
