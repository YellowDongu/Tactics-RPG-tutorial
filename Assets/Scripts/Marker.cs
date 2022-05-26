using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] Transform marker;
    [SerializeField] Grid targetGrid;
    [SerializeField] float elevation = 2f;

    MouseInput mouseInput;

    Vector2Int currentPosition;

    bool active;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
    }

    private void Update()
    {
        if (active != mouseInput.active)
        {
            active = mouseInput.active;
            marker.gameObject.SetActive(active);
        }
        if (active == false)
        {
            return;
        }
        if (currentPosition != mouseInput.positionOnGrid)
        {
            currentPosition = mouseInput.positionOnGrid;
            UpdateMarker();
        }
    }

    private void UpdateMarker()
    {
        if(targetGrid.CheckBoundry(currentPosition) == false)
        {
            return;
        }
        Vector3 wordlPosition = targetGrid.GetWorldPosition(currentPosition.x, currentPosition.y, true);
        wordlPosition.y += elevation;
        marker.position = wordlPosition;



    }
}
