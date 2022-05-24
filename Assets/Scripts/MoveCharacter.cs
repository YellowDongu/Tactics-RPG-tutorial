using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] GridObject targetCharacter;
    [SerializeField] LayerMask terrainLayerMask;

    [SerializeField] GridHighlight gridHighlight;

    Pathfinding pathfinding;
    List<PathNode> path;


    private void Start()
    {
        pathfinding = targetGrid.GetComponent<Pathfinding>();
        CheckWalkableTerrain();
    }

    private void CheckWalkableTerrain()
    {
        List<PathNode> walkableNodes = new List<PathNode>();
        pathfinding.CalculateWalkableNodes(targetCharacter.positionOnGrid.x, targetCharacter.positionOnGrid.y, targetCharacter.GetComponent<Character>().movementPoints, ref walkableNodes);
        gridHighlight.Highlight(walkableNodes);
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

                //path = pathfinding.FindPath(targetCharacter.positionOnGrid.x, targetCharacter.positionOnGrid.y, gridposition.x, gridposition.y);

                path = pathfinding.TraceBackPath(gridposition.x, gridposition.y);
                path.Reverse();

                if (path == null)
                {
                    return;
                }
                if (path.Count == 0)
                {
                    return;
                }

                targetCharacter.GetComponent<Movement>().Move(path);
            }
        }
    }
}
