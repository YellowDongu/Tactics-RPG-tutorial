using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    //��ǥ�� ��ȣ�ۿ� �ϴ� ��ũ��Ʈ. ĳ���͸� �����ϴ� �ִ� �ٸ� ��ũ��Ʈ��!
    //ĳ���͸� �����Ŵ� ���ִ� �ִϱ� ĳ���Ϳ� �ٿ�����
    //�ַ� �����ӿ� ���� ������
    public Grid targetGrid;//��ȣ�ۿ��� �׸���
    public Vector2Int positionOnGrid;//�׸��� ���� ��ǥ ǥ�� �� ����


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //������ǥ���� �׸��� ��ǥ�� ��ȯ
        positionOnGrid = targetGrid.GetGridPosition(transform.position);
        //��� ���� �÷�����
        targetGrid.PlaceObject(positionOnGrid, this);
        //�׸��� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 pos = targetGrid.GetWorldPosition(positionOnGrid.x, positionOnGrid.y, true);
        //�������� ��ġ�� ��Ȯ�ϰ� ���� �÷����� �����Ѵ�.
        transform.position = pos;
    }
}
