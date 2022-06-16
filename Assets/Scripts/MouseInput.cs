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
    //ǥ���� �ؽ�Ʈ ���� ������Ʈ ����(TMP)
    [SerializeField] TMPro.TextMeshProUGUI positionOnScreen;

    private void Update()
    {
        //�׻� �۵��ϵ� ������ �ƴ� ��ü�� ���콺�� �ö� ��� �۵� ������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            //��Ŀ�� ��ǥ �ȿ� �ִٰ� ����
            active = true;
            Vector2Int hitPosition = targetGrid.GetGridPosition(hit.point);
            if(hitPosition != positionOnGrid)
            {
                positionOnGrid = hitPosition;
                //������ ǥ��(�ؽ�Ʈ)
                positionOnScreen.text = "Position : " + positionOnGrid.x.ToString() + "," + positionOnGrid.y.ToString();
            }
        }
        else
        {

            active = false;
            //��ǥ�� null�϶� ǥ���ϴ� �ؽ�Ʈ
            positionOnScreen.text = "Outside";
        }
    }
}
