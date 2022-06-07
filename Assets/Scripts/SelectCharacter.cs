using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    MouseInput mouseInput;
    CommandMenu commandMenu;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
        commandMenu = GetComponent<CommandMenu>();
    }

    public Character selected;
    GridObject hoverOverGridObject;
    public Character hoverOverCharacter;
    Vector2Int positionOnGrid = new Vector2Int(-1, -1);
    [SerializeField] Grid targetGrid;

    private void Update()
    {
        HoverOverObject();

        selectInput();
        DeselectInput();
    }

    private void HoverOverObject()
    {
        if (positionOnGrid != mouseInput.positionOnGrid)
        {
            positionOnGrid = mouseInput.positionOnGrid;
            hoverOverGridObject = targetGrid.GetPlacedObject(positionOnGrid);
            if (hoverOverGridObject != null)
            {
                hoverOverCharacter = hoverOverGridObject.GetComponent<Character>();
            }
            else
            {
                hoverOverCharacter = null;
            }
        }
    }

    private void DeselectInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selected = null;
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        if(selected != null)
        {
            commandMenu.OpenPanel();
        }
        else
        {
            commandMenu.ClosePanel();
        }
    }

    private void selectInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hoverOverCharacter != null && selected == null)
            {
                selected = hoverOverCharacter;
            }
        }
        UpdatePanel();
    }

    internal void Deselect()
    {
        selected = null;
    }
}
