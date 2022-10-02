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
        //캐릭터가 갈 수 있는 곳을 계산하고 표시해준다.

        GridObject gridObject = targetCharacter.GetComponent<GridObject>();
        //계산된 노드들을 넣을 애
        List<PathNode> walkableNodes = new List<PathNode>();
        //초기화해서 오류 나지 않게
        pathfinding.Clear();
        //갈 수 있는 곳을 계산해주는 애
        pathfinding.CalculateWalkableNodes(gridObject.positionOnGrid.x, gridObject.positionOnGrid.y, targetCharacter.movementPoints, ref walkableNodes);
        //기존에 있던 애들을 안보이게 한다
        gridHighlight.Hide();
        //위의 계산을 받아서 그래픽으로 보여주는 애
        gridHighlight.Highlight(walkableNodes);
    }

    public List<PathNode> GetPath(Vector2Int from)
    {
        //정제가 완료된 리스트를 여기다가 저장한다.
        List<PathNode> path = pathfinding.TraceBackPath(from.x, from.y);
        //경로 없을 시 아무것도 안뱉음
        if (path == null)
        {
            return null;
        }
        if (path.Count == 0)
        {
            return null;
        }
        //메소드에서 리버스 안시켜줬으니 여기서 리버스
        path.Reverse();

        return path;//경로 받아라
    }

    public bool CheckOccupied(Vector2Int positionOnGrid)
    {
        return targetGrid.CheckOccupied(positionOnGrid);
    }
}
