using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    //��Ŀ�� �� ������Ʈ ��ġ
    [SerializeField] Transform marker;
    [SerializeField] Grid targetGrid;
    [SerializeField] float elevation = 2f;

    MouseInput mouseInput;

    Vector2Int currentPosition;
    //��Ŀ ��Ȱ��/Ȱ��ȭ
    bool active;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
    }

    private void Update()
    {
        //���콺 ��ǲ ��Ȱ��ȭ�ε� �ڽ��� Ȱ��ȭ
        if (active != mouseInput.active)
        {
            //��Ƽ��� ��������
            active = mouseInput.active;
            //��Ŀ�� ��Ƽ��
            marker.gameObject.SetActive(active);
        }
        //��Ȱ��ȭ��
        if (active == false)
        {
            return;//��ŵ
        }
        //���콺�� ��ǥ�� �ڽ��� ��ǥ�� �ٸ���
        if (currentPosition != mouseInput.positionOnGrid)
        {
            //������
            currentPosition = mouseInput.positionOnGrid;
            //Ŀ���� �Ű��ְ�
            UpdateMarker();
        }
    }

    private void UpdateMarker()
    {
        //�� ���� �ƴ� ��
        if(targetGrid.CheckBoundry(currentPosition) == false)
        {
            return;
        }
        //�׸��� ��ǥ�� ���� ��ǥ�� �ٲ�
        Vector3 wordlPosition = targetGrid.GetWorldPosition(currentPosition.x, currentPosition.y, true);
        //���� �������ְ�
        wordlPosition.y += elevation;
        //������� �������ش�.
        marker.position = wordlPosition;
    }
}
