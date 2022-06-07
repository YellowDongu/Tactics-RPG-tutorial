using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    Node[,] grid;
    public int width = 25;
    public int length = 25;
    [SerializeField] float cellSize = 1f;
    [SerializeField] LayerMask obstracleLayer;
    [SerializeField] LayerMask terrainLayer;

    private void Awake()
    {
        GenerateGrid();
    }

    public void PlaceObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        //�׸��� ���� �÷��ִ� ����
        //���� ��ǥ�� �����ϴ������� �����
        if(CheckBoundry(positionOnGrid) == true)
        {
            //�ش� ��ǥ�� �ִ� �ָ� ����ִ� ����� �׸��������Ʈ�� ����Ѵ�.
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = gridObject;
        }
        else
        {
            Debug.Log("Object at outside the boundaries!");
        }
    }

    internal void RemoveObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        //placeobject�� ��Ŀ������ ����
        if (CheckBoundry(positionOnGrid) == true)
        {
            //�ٸ� �̹����� ��ϵ� �ָ� ��� ���� ��Ű�°��� ��
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = null;
        }
        else
        {
            Debug.Log("Object at outside the boundaries!");
        }
    }

    public List<Vector3> ConvertPathNodesToWorldPositions(List<PathNode> path)
    {
        //pathfinding�� ��� ��ǥ�� ���� ��ǥ�� ��ȯ -- ��� ������ ���
        List<Vector3> worldPositions = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            worldPositions.Add(GetWorldPosition(path[i].pos_x, path[i].pos_y, true));
        }

        return worldPositions;
    }

    public bool CheckBoundry(Vector2Int positionOnGrid)
    {
        //��� üũ(����2��)
        if (positionOnGrid.x < 0 || positionOnGrid.x >= length)
        {
            return false;
        }
        if (positionOnGrid.y < 0 || positionOnGrid.y >= width)
        {
            return false;
        }

        return true;
    }

    internal bool CheckBoundry(int posX, int posY)
    {
        //��� üũ(��ǥ��)
        if (posX < 0 || posX >= length)
        {
            return false;
        }
        if (posY < 0 || posY >= width)
        {
            return false;
        }

        return true;
    }

    private void GenerateGrid()
    {
        //2���� ��� Ȱ��ȭ �� ��� ������� ����
        grid = new Node[length, width];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                //��ũ��Ʈ�� ���ڲ��� �ٿ��ش�. �׷��� ��帶�� �ٸ� �Ӽ��� ���� �� �ִ�.
                grid[x, y] = new Node();
            }
        }
        CalculateElevation();
        CheckPassableTerrain();
    }

    private void CalculateElevation()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                //x�� y ��ǥ�� ������ǥ�� ��ȯ�� �ű⿡ ���� ������ ���.
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f, Vector3.down);
                RaycastHit hit;//�̸� ����
                if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer))//������ �ش� ���̾ �ɷȴ�!
                {
                    grid[x, y].elevation = hit.point.y;//���̰��� ��忡�� ����
                }
            }
        }

    }

    internal GridObject GetPlacedObject(Vector2Int gridposition)
    {
        if (CheckBoundry(gridposition))//��ǥ ���� üũ
        {
            GridObject gridObject = grid[gridposition.x, gridposition.y].gridObject;
            return gridObject;
        }
        return null;
    }

    private void CheckPassableTerrain()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                //���� ��ǥ �O��
                Vector3 worldPosition = GetWorldPosition(x, y, true);
                //�簢��(ĭ�� �����ŭ) ���� �ִ� ��ֹ����̾� ������Ʈ üũ
                bool passable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * cellSize, Quaternion.identity, obstracleLayer);
                grid[x, y].passable = passable; //������ ��忡�� �����Ѵ�.
            }
        }
    }
    public bool CheckWalkable(int pos_x, int pos_y)
    {
        //�ٸ� �ֵ鿡�Լ� ��ǥ�� �ް� �ش� ��忡�Լ� �� �� �ֳ� ���� ������ �޾Ƽ� �ش�.
        return grid[pos_x, pos_y].passable;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += cellSize / 2;
        worldPosition.z += cellSize / 2;
        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / cellSize), (int)(worldPosition.z / cellSize));
        return positionOnGrid;
    }

    //��ǥ �߽� ǥ��(�����ͻ󿡼���)
    private void OnDrawGizmos()
    {
        if (grid == null)//�׸��� ������
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    //����Ʈ -- ��ǥ�� ���� ��ǥ�� ��ȯ�ؼ� ǥ��
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.DrawCube(pos, Vector3.one / 4);

                }
            }

        }
        else
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    //��忡�Լ� ���� �޴´�
                    //������ ��ǥ ��ȯ��
                    Vector3 pos = GetWorldPosition(x, y, true);
                    //�޾Ҵ�. ���� �ֳ� ���� ������. �̰� ���� �׸���.
                    Gizmos.color = grid[x, y].passable ? Color.white : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }

    }
    
    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        //x�� y ��ǥ�� �޾Ƽ� ���� ��ǥ�� ��ȯ���ش�.
        return new Vector3(x * cellSize, elevation == true ? grid[x, y].elevation : 0f, y * cellSize);
    }
}
