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
    //표시할 텍스트 게임 오브젝트 지정(TMP)
    [SerializeField] TMPro.TextMeshProUGUI positionOnScreen;

    private void Update()
    {
        //항상 작동하되 지상이 아닌 물체에 마우스가 올라간 경우 작동 중지함
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            //마커에 좌표 안에 있다고 전송
            active = true;
            Vector2Int hitPosition = targetGrid.GetGridPosition(hit.point);
            if(hitPosition != positionOnGrid)
            {
                positionOnGrid = hitPosition;
                //포지션 표시(텍스트)
                positionOnScreen.text = "Position : " + positionOnGrid.x.ToString() + "," + positionOnGrid.y.ToString();
            }
        }
        else
        {

            active = false;
            //좌표가 null일때 표시하는 텍스트
            positionOnScreen.text = "Outside";
        }
    }
}
