using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] LayerMask terrainLayerMask;

    [SerializeField] GridObject hoveringOver;
    [SerializeField] SelectableGridObject selectedObject;

    Vector2Int currentGridPosition = new Vector2Int(-1, -1);

    private void Update()
    {
        HoverOverObjectCheck();
        selectObject();
        DeselectObject();
    }

    private void DeselectObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectedObject = null;
        }
    }

    private void selectObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hoveringOver == null)
            {
                return;
            }
            selectedObject = hoveringOver.GetComponent<SelectableGridObject>();
        }
    }

    private void HoverOverObjectCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);
            if(gridposition == currentGridPosition)
            {
                return;
            }
            currentGridPosition = gridposition;
            GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
            hoveringOver = gridObject;
        }
    }
}
