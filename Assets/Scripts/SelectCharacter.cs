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
        //마우스의 위치정보가 있으면 그거를 가져온다
        if (positionOnGrid != mouseInput.positionOnGrid)
        {
            //현재 좌표를 위치정보로 설정
            positionOnGrid = mouseInput.positionOnGrid;
            //노드에게서 위에 있는 오브젝트 정보를 받아온다.
            hoverOverGridObject = targetGrid.GetPlacedObject(positionOnGrid);
            if (hoverOverGridObject != null)
            {
                //호버링 오브젝트의 케릭터 정보를 가져온다.
                hoverOverCharacter = hoverOverGridObject.GetComponent<Character>();
            }
            else
            {
                //커서 위에서 오브젝트가 사라지면 초기화
                hoverOverCharacter = null;
            }
        }
    }

    private void DeselectInput()
    {
        //패널 열린 상태에서 좌클릭을 하면 패널이 꺼진다.
        if (Input.GetMouseButtonDown(1))
        {
            selected = null;
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        //선택된 애가 있으면 명령패널 열어준다
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
        //인풋 넣어주는 애
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
