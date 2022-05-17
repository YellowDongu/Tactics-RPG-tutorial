using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    [SerializeField] Grid targetGrid;
    [SerializeField] LayerMask terrainLayerMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
            {
                Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);
                GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
                if(gridObject == null)
                {
                    Debug.Log("x=" + gridposition.x + "y=" + gridposition.y + "is empty");
                }
                else
                {
                    Debug.Log("x=" + gridposition.x + "y=" + gridposition.y + gridObject.GetComponent<Character>().Name);
                }
            }
        }
    }



}
