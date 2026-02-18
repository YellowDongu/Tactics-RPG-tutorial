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
        //그리드 위에 올려주는 아이
        //먼저 좌표가 실존하는지부터 물어보자
        if(CheckBoundry(positionOnGrid) == true)
        {
            //해당 좌표에 있는 애를 밟고있는 노드의 그리드오브젝트로 등록한다.
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = gridObject;
        }
        else
        {
            Debug.Log("Object at outside the boundaries!");
        }
    }

    internal void RemoveObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        //placeobject와 매커니즘이 같다
        if (CheckBoundry(positionOnGrid) == true)
        {
            //다만 이번에는 등록된 애를 등록 해제 시키는거일 뿐
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = null;
        }
        else
        {
            Debug.Log("Object at outside the boundaries!");
        }
    }

    public List<Vector3> ConvertPathNodesToWorldPositions(List<PathNode> path)
    {
        //pathfinding의 노드 좌표를 월드 좌표로 변환 -- 경로 노드들이 대상
        List<Vector3> worldPositions = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            worldPositions.Add(GetWorldPosition(path[i].pos_x, path[i].pos_y, true));
        }

        return worldPositions;
    }

    internal bool CheckOccupied(Vector2Int positionOnGrid)
    {
        return GetPlacedObject(positionOnGrid) != null;
    }

    public bool CheckBoundry(Vector2Int positionOnGrid)
    {
        //경계 체크(벡터2용)
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
        //경계 체크(좌표용)
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
        //2차원 노드 활성화 및 노드 갯수대로 생성
        grid = new Node[length, width];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                //스크립트를 각자껄로 붙여준다. 그래야 노드마다 다른 속성을 붙일 수 있다.
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
                //x와 y 좌표를 월드좌표로 변환해 거기에 수직 광선을 쏜다.
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f, Vector3.down);
                RaycastHit hit;//이름 지정
                if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayer))//광선에 해당 레이어가 걸렸다!
                {
                    grid[x, y].elevation = hit.point.y;//높이값을 노드에게 전달
                }
            }
        }

    }

    internal GridObject GetPlacedObject(Vector2Int gridposition)
    {
        if (CheckBoundry(gridposition))//좌표 실존 체크
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
                //월드 좌표 찿고
                Vector3 worldPosition = GetWorldPosition(x, y, true);
                //사각형(칸의 사이즈만큼) 내에 있는 장애물레이어 오브젝트 체크
                bool passable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * cellSize, Quaternion.identity, obstracleLayer);
                grid[x, y].passable = passable; //정보를 노드에게 전달한다.
            }
        }
    }
    public bool CheckWalkable(int pos_x, int pos_y)
    {
        //다른 애들에게서 좌표를 받고 해당 노드에게서 갈 수 있나 없나 정보를 받아서 준다.
        return grid[pos_x, pos_y].passable;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += cellSize / 2;
        worldPosition.z += cellSize / 2;
        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / cellSize), (int)(worldPosition.z / cellSize));
        return positionOnGrid;
    }

    //좌표 중심 표시(에디터상에서만)
    private void OnDrawGizmos()
    {
        if (grid == null)//그리드 없으면
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    //디폴트 -- 좌표를 월드 좌표로 변환해서 표시
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
                    //노드에게서 뭔가 받는다
                    //그전에 좌표 변환좀
                    Vector3 pos = GetWorldPosition(x, y, true);
                    //받았다. 갈수 있나 없나 정보다. 이걸 토대로 그린다.
                    Gizmos.color = grid[x, y].passable ? Color.white : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }

    }
    
    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        //x와 y 좌표를 받아서 월드 좌표로 변환해준다.
        return new Vector3(x * cellSize, elevation == true ? grid[x, y].elevation : 0f, y * cellSize);
    }
}
