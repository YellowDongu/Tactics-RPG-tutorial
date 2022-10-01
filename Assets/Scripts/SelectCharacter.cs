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
        //���콺�� ��ġ������ ������ �װŸ� �����´�
        if (positionOnGrid != mouseInput.positionOnGrid)
        {
            //���� ��ǥ�� ��ġ������ ����
            positionOnGrid = mouseInput.positionOnGrid;
            //��忡�Լ� ���� �ִ� ������Ʈ ������ �޾ƿ´�.
            hoverOverGridObject = targetGrid.GetPlacedObject(positionOnGrid);
            if (hoverOverGridObject != null)
            {
                //ȣ���� ������Ʈ�� �ɸ��� ������ �����´�.
                hoverOverCharacter = hoverOverGridObject.GetComponent<Character>();
            }
            else
            {
                //Ŀ�� ������ ������Ʈ�� ������� �ʱ�ȭ
                hoverOverCharacter = null;
            }
        }
    }

    private void DeselectInput()
    {
        //�г� ���� ���¿��� ��Ŭ���� �ϸ� �г��� ������.
        if (Input.GetMouseButtonDown(1))
        {
            selected = null;
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        //���õ� �ְ� ������ ����г� �����ش�
        if(selected != null)
        {
            commandMenu.OpenPanel(selected.GetComponent<CharacterTurn>());
        }
        else
        {
            commandMenu.ClosePanel();
        }
    }

    private void selectInput()
    {
        //��ǲ �־��ִ� ��
        if (Input.GetMouseButtonDown(0))
        {
            if (hoverOverCharacter != null && selected == null)
            {
                selected = hoverOverCharacter;
            }
        }
        UpdatePanel();
    }

    public void Deselect()
    {
        selected = null;
    }
}
