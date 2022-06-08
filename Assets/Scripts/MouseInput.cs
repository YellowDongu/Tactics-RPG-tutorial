using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Grid targetGrid;
    [SerializeField] LayerMask terrainLayerMask;

    public Vector2Int positionOnGrid;

    public bool active;


    private void Start()
    {
        
    }



    private void Update()
    {
        //항상 작동하되 지상이 아닌 물체에 마우스가 올라간 경우 작동 중지함
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            active = true;
            Vector2Int hitPosition = targetGrid.GetGridPosition(hit.point);
            if(hitPosition != positionOnGrid)
            {
                positionOnGrid = hitPosition;
            }
        }
        else
        {
            active = false;
        }
    }
}
