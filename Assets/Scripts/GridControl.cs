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
    //���� ��ǥ, �⺻���δ� -1,-1�� �׸��� �ۿ� �⺻�� ��������.
    Vector2Int currentGridPosition = new Vector2Int(-1, -1);

    private void Update()
    {
        HoverOverObjectCheck();
        selectObject();
        DeselectObject();
    }

    private void DeselectObject()
    {
        //���콺 ��Ŭ��!
        if (Input.GetMouseButtonDown(1))
        {
            //������ �����Ѵ�
            selectedObject = null;
        }
    }

    private void selectObject()
    {
        //���콺 ������ Ŭ��!(��ư �ٿ�ÿ���)
        if (Input.GetMouseButtonDown(0))
        {
            //�ش� �׸��忡 ��ϵ� ������Ʈ�� ��� ȣ���� ������ ��ϵ� ������Ʈ�� ����
            if (hoveringOver == null)
            {
                return;//�׷� ���
            }
            //�ƴϸ� selectablegridobject ��ũ��Ʈ�� �ִ��� Ȯ���ϰ� �����´�.
            //���� ��ũ��Ʈ�� �������°�, �̰�� ���� ������ �ֵ��� �ɷ�����.
            selectedObject = hoveringOver.GetComponent<SelectableGridObject>();
        }
    }

    private void HoverOverObjectCheck()
    {
        //���콺 ����Ʈ�� ���� �ɸ�����(��������) üũ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //���� ���콺����Ʈ �ڿ� �� ���̾ �ɷȴ�
        if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            //�� ������ ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ
            Vector2Int gridposition = targetGrid.GetGridPosition(hit.point);
            //�׸��� ��ǥ�� ���� = �������� �ʾҴ� = �ƹ��͵� ���ص� �ȴ�
            if(gridposition == currentGridPosition)
            {
                return;
            }
            //�׸��� ��ǥ�� �ٸ��� => ���� ��ǥ�� ������ �׸��� ��ǥ�� �ٲ۴�
            currentGridPosition = gridposition;
            //�׸��� ������Ʈ�� ��ϵ� ������Ʈ�� �ִ��� Ȯ��
            GridObject gridObject = targetGrid.GetPlacedObject(gridposition);
            //������ �׳��� ��ϵȴ�
            hoveringOver = gridObject;
        }
    }
}
