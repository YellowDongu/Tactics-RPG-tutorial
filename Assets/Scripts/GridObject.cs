using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    //좌표랑 상호작용 하는 스크립트. 캐릭터를 정의하는 애는 다른 스크립트에!
    //캐릭터를 포지셔닝 해주는 애니깐 캐릭터에 붙여넣자
    //주로 움직임에 관한 아이임
    public Grid targetGrid;//상호작용할 그리드
    public Vector2Int positionOnGrid;//그리드 상의 좌표 표시 및 저장


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //월드좌표에서 그리드 좌표로 변환
        positionOnGrid = targetGrid.GetGridPosition(transform.position);
        //노드 위에 올려놓자
        targetGrid.PlaceObject(positionOnGrid, this);
        //그리드 좌표를 월드 좌표로 변환
        Vector3 pos = targetGrid.GetWorldPosition(positionOnGrid.x, positionOnGrid.y, true);
        //물리적인 위치를 정확하게 위에 올려놓게 보정한다.
        transform.position = pos;
    }
}
