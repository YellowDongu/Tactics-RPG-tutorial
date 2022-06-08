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
    //현재 좌표, 기본으로는 -1,-1로 그리드 밖에 기본을 지정하자.
    Vector2Int currentGridPosition = new Vector2Int(-1, -1);

    private void Update()
    {
        HoverOverObjectCheck();
        selectObject();
        DeselectObject();
    }

    private void DeselectObject()
    {
        //마우스 좌클릭!
        if (Input.GetMouseButtonDown(1))
        {
            //선택을 해제한다
            selectedObject = null;
        }
    }

    private void selectObject()
    {
        //마우스 오른쪽 클릭!(버튼 다운시에만)
        if (Input.GetMouseButtonDown(0))
        {
            //해당 그리드에 등록된 오브젝트가 없어서 호버링 오버에 등록된 오브젝트가 없다
            if (hoveringOver == null)
            {
                return;//그럼 놀아
            }
            //아니면 selectablegridobject 스크립트가 있는지 확인하고 가져온다.
            //물론 스크립트를 가져오는거, 이결로 선택 가능한 애들을 걸러낸다.
            selectedObject = hoveringOver.GetComponent<SelectableGridObject>();
        }
    }

    private void HoverOverObjectCheck()
    {
        //마우스 포인트에 뭐가 걸리는지(광선쏴서) 체크
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //만약 마우스포인트 뒤에 땅 레이어가 걸렸다
        if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            //그 지점의 월드 좌표를 그리드 좌표로 변환
            Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);
            //그리드 좌표가 같다 = 움직이지 않았다 = 아무것도 안해도 된다
            if(gridposition == currentGridPosition)
            {
                return;
            }
            //그리드 좌표가 다르다 => 현재 좌표를 지금의 그리드 좌표로 바꾼다
            currentGridPosition = gridposition;
            //그리드 오브젝트에 등록된 오브젝트가 있는지 확인
            GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
            //있으면 그놈이 등록된다
            hoveringOver = gridObject;
        }
    }
}
