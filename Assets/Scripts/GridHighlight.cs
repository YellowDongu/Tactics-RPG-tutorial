using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    Grid grid;
    [SerializeField] GameObject movePoint;
    List<GameObject> movePointGOs;
    [SerializeField] GameObject movePointsContainer;


    [SerializeField] List<Vector2Int> testTargetPosition;


    void Awake()
    {
        grid = GetComponent<Grid>();
        movePointGOs = new List<GameObject>();
        //Highlight(testTargetPosition);
    }

    private GameObject CreateMovePointHighlightObject()
    {
        GameObject go = Instantiate(movePoint);
        movePointGOs.Add(go);
        go.transform.SetParent(movePointsContainer.transform);
        return go;
    }

    public void Highlight(List<Vector2Int> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].x, positions[i].y, GetMovePointGO(i));
        }
    }
    public void Highlight(List<PathNode> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].pos_x, positions[i].pos_y, GetMovePointGO(i));
        }
    }

    private GameObject GetMovePointGO(int i)
    {
        if(movePointGOs.Count < i)
        {
            return movePointGOs[i];
        }

        GameObject newHighlightObject = CreateMovePointHighlightObject();
        return newHighlightObject;
    }

    public void Highlight(int posX, int posY, GameObject highlightObject)
    {
        Vector3 position = grid.GetWorldPosition(posX, posY, true);
        position += Vector3.up * 0.2f;
        highlightObject.transform.position = position;
    }
}
