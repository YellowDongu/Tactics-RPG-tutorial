using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] GridHighlight gridHighlight;

    Pathfinding pathfinding;


    private void Start()
    {
        pathfinding = targetGrid.GetComponent<Pathfinding>();
    }

    public void CheckWalkableTerrain(Character targetCharacter)
    {
        GridObject gridObject = targetCharacter.GetComponent<GridObject>();
        List<PathNode> walkableNodes = new List<PathNode>();
        pathfinding.Clear();
        pathfinding.CalculateWalkableNodes(gridObject.positionOnGrid.x, gridObject.positionOnGrid.y, targetCharacter.movementPoints, ref walkableNodes);
        gridHighlight.Hide();
        gridHighlight.Highlight(walkableNodes);
    }

    public List<PathNode> GetPath(Vector2Int from)
    {
        List<PathNode> path = pathfinding.TraceBackPath(from.x, from.y);
        path.Reverse();

        if (path == null)
        {
            return null;
        }
        if (path.Count == 0)
        {
            return null;
        }
        return path;
    }
}
