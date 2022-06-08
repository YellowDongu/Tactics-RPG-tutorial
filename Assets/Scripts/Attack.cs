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
        //공격할때 몸을 틀어주는 역할 + 애니메이션 재생을 하는 아이
        RotateCharacter(targetGridObjsect.transform.position);
        characterAnimator.Attack();
    }

    private void RotateCharacter(Vector3 towards)
    {
        //공격할때 몸을 틀어주는 역할
        Vector3 direction = (towards - transform.position).normalized;
        //물론 하늘 보지 않게 y값은 없애줄거임
        direction.y = 0;
        //좌표값으로 봐야하는 곳을 알려주고 알아서 틀도록 함
        transform.rotation = Quaternion.LookRotation(direction);

    }
}
