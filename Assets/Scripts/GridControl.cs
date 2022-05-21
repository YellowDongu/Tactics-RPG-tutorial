using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] LayerMask terrainLayerMask;

    Pathfinding pathfinding;
    Vector2Int currentPosition = new Vector2Int();
    List<PathNode> path;


    private void Start()
    {
        pathfinding = targetGrid.GetComponent<Pathfinding>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
            {
                Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);

                path = pathfinding.FindPath(currentPosition.x, currentPosition.y, gridposition.x, gridposition.y);

                currentPosition = gridposition;



                /*
                GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
                if(gridObject == null)
                {
                    Debug.Log("x=" + gridposition.x + "y=" + gridposition.y + "is empty");
                }
                else
                {
                    Debug.Log("x=" + gridposition.x + "y=" + gridposition.y + gridObject.GetComponent<Character>().Name);
                }*/
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(path == null)
        {
            return;
        }
        if(path.Count == 0)
        {
            return;
        }
        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(targetGrid.GetWorldPosition(path[i].pos_x, path[i].pos_y, true),
                targetGrid.GetWorldPosition(path[i + 1].pos_x, path[i + 1].pos_y, true));
        }
    }

}
