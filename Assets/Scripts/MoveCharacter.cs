using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] GridObject targetCharacter;
    [SerializeField] LayerMask terrainLayerMask;

    Pathfinding pathfinding;
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
            if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
            {
                Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);

                path = pathfinding.FindPath(targetCharacter.positionOnGrid.x, targetCharacter.positionOnGrid.y, gridposition.x, gridposition.y);

                targetCharacter.GetComponent<Movement>().Move(path);


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
}
