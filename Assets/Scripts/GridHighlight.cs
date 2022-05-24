using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    Grid grid;
    [SerializeField] GameObject highlightPoint;
    [SerializeField] GameObject Container;

    List<GameObject> highlightPointsGOs;



    void Awake()
    {
        grid = GetComponentInParent<Grid>();
        highlightPointsGOs = new List<GameObject>();
        //Highlight(testTargetPosition);
    }

    private GameObject CreateHighlightObject()
    {
        GameObject go = Instantiate(highlightPoint);
        highlightPointsGOs.Add(go);
        go.transform.SetParent(Container.transform);
        return go;
    }

    public void Highlight(List<Vector2Int> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].x, positions[i].y, GetHighlightPointGO(i));
        }
    }
    public void Highlight(List<PathNode> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].pos_x, positions[i].pos_y, GetHighlightPointGO(i));
        }
    }

    private GameObject GetHighlightPointGO(int i)
    {
        if(highlightPointsGOs.Count < i)
        {
            return highlightPointsGOs[i];
        }

        GameObject newHighlightObject = CreateHighlightObject();
        return newHighlightObject;
    }

    public void Highlight(int posX, int posY, GameObject highlightObject)
    {
        Vector3 position = grid.GetWorldPosition(posX, posY, true);
        position += Vector3.up * 0.2f;
        highlightObject.transform.position = position;
    }
}
