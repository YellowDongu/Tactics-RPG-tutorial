using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    GridObject gridObject;
    CharacterAnimator characterAnimator;


    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();

    }


    public void AttackPosition(GridObject targetGridObjsect)
    {
        //�����Ҷ� ���� Ʋ���ִ� ���� + �ִϸ��̼� ����� �ϴ� ����
        RotateCharacter(targetGridObjsect.transform.position);
        characterAnimator.Attack();
    }

    private void RotateCharacter(Vector3 towards)
    {
        //�����Ҷ� ���� Ʋ���ִ� ����
        Vector3 direction = (towards - transform.position).normalized;
        //���� �ϴ� ���� �ʰ� y���� �����ٰ���
        direction.y = 0;
        //��ǥ������ �����ϴ� ���� �˷��ְ� �˾Ƽ� Ʋ���� ��
        transform.rotation = Quaternion.LookRotation(direction);

    }
}
