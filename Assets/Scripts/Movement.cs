using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //ĳ���͵��� �������� �����Ѵ�. �н����ε��̶� �׸���� ���� �ֵ��� �߰��ϴ� ��
    GridObject gridObject;
    CharacterAnimator characterAnimator;

    List<Vector3> pathWorldPositions;//��� ��ǥ

    public bool IS_MOVING
    {
        get
        {
            if(pathWorldPositions == null)//���� ��ΰ� ������ false ���
            {
                return false;
            }
            return pathWorldPositions.Count > 0;//������ true
        }
    }

    [SerializeField] float moveSpeed = 1f;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
    }

    public void Move(List<PathNode> path)
    {
        //�����϶��� ��ŵ���Ѽ� ���������� �ű� �� ���� �������� ������ �ǽ� --���Ұ���
        if (IS_MOVING)
        {
            SkipAnimation();
        }

        //��ο� �ִ� ��� ��ǥ���� ��� ���� ��ǥ�� ��ȯ
        pathWorldPositions =  gridObject.targetGrid.ConvertPathNodesToWorldPositions(path);
        //������ �ָ� ���� ��ǥ ��忡�� ��� ����
        gridObject.targetGrid.RemoveObject(gridObject.positionOnGrid, gridObject);

        //��忡 �����Ҷ����� ����� ��ǥ�� �ҷ��� ĳ������ ��ǥ�� ��� ��ǥ�� ������Ʈ
        gridObject.positionOnGrid.x = path[path.Count - 1].pos_x;
        gridObject.positionOnGrid.y = path[path.Count - 1].pos_y;
        //��ϵ� ��
        gridObject.targetGrid.PlaceObject(gridObject.positionOnGrid, gridObject);
        //���� ��ǥ�� �� Ʋ��
        RotateCharacter(transform.position, pathWorldPositions[0]);
        //�ִϸ��̼ǵ� ���
        characterAnimator.StartMoving();
    }


    private void RotateCharacter(Vector3 originPosition, Vector3 destinationPosition)
    {
        //�� ������
        //��������� �������� ���� ��ֶ������ ���� 1 �Ʒ��� ������
        Vector3 direction = (destinationPosition - originPosition).normalized;
        //���� �ϴú��� ���� ����
        direction.y = 0;
        //����!
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        //���� ��ϵ� ��ΰ� ���� = �׷� �ƹ��͵� ���Ѵ�
        if(pathWorldPositions == null)
        {
            return;
        }
        if(pathWorldPositions.Count == 0)//����
        {
            return;
        }
        //ĳ���͸� ���������� �����̰��ϴ� ����
        transform.position = Vector3.MoveTowards(transform.position, pathWorldPositions[0], moveSpeed * Time.deltaTime);
        //��� ��忡 �ٴٸ�(üũ����Ʈ�� ������)
        if(Vector3.Distance(transform.position, pathWorldPositions[0]) < 0.05f)
        {
            //üũ����Ʈ �н� -- �ٴٸ� ��� ���� �� ���� ���� ���
            pathWorldPositions.RemoveAt(0);
            if(pathWorldPositions.Count == 0)//���̻� ��� ��尡 ����
            {
                characterAnimator.StopMoving();//�׷� ����������
            }
            else//��ΰ� ���ҳ�? �׷� �� ��η� ���� Ʋ�����
            {
                RotateCharacter(transform.position, pathWorldPositions[0]);
            }
        }
    }

    public void SkipAnimation()
    {
        if(pathWorldPositions.Count < 2)//�̹� �������ֳ�? �׷� ������
        {
            return;
        }
        //����� ������ ����� ��ġ�� �ڷ���Ʈ!
        transform.position = pathWorldPositions[pathWorldPositions.Count - 1];
        //�� ������ ���ϰ� �������� ��ĭ �� ��带 �ҷ���
        Vector3 originPosition = pathWorldPositions[pathWorldPositions.Count - 2];
        //����� ������ ��� �ҷ���
        Vector3 destinationPosition = pathWorldPositions[pathWorldPositions.Count - 1];
        //�� Ʋ��
        RotateCharacter(originPosition, destinationPosition);
        //�޾ƿ� ��� ���δ� �����ع��� �ʿ����
        pathWorldPositions.Clear();
        //����!
        characterAnimator.StopMoving();
    }
}
