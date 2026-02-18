using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    //마커로 쓸 오브젝트 배치
    [SerializeField] Transform marker;
    [SerializeField] Grid targetGrid;
    [SerializeField] float elevation = 2f;

    MouseInput mouseInput;

    Vector2Int currentPosition;
    //마커 비활성/활성화
    bool active;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
    }

    private void Update()
    {
        //마우스 인풋 비활성화인데 자신은 활성화
        if (active != mouseInput.active)
        {
            //액티브로 돌려놓음
            active = mouseInput.active;
            //마커도 액티브
            marker.gameObject.SetActive(active);
        }
        //비활성화시
        if (active == false)
        {
            return;//스킵
        }
        //마우스의 좌표와 자신의 좌표가 다르면
        if (currentPosition != mouseInput.positionOnGrid)
        {
            //맞춰줌
            currentPosition = mouseInput.positionOnGrid;
            //커서도 옮겨주고
            UpdateMarker();
        }
    }

    private void UpdateMarker()
    {
        //맵 안이 아닐 떄
        if(targetGrid.CheckBoundry(currentPosition) == false)
        {
            return;
        }
        //그리드 좌표를 월드 좌표로 바꿈
        Vector3 wordlPosition = targetGrid.GetWorldPosition(currentPosition.x, currentPosition.y, true);
        //고도도 보정해주고
        wordlPosition.y += elevation;
        //적용시켜 움직여준다.
        marker.position = wordlPosition;
    }
}
