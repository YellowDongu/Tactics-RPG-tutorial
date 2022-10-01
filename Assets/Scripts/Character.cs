using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Int2Val
{
    public int current;
    public int max;

    public bool canGoNegative;

    public Int2Val(int current, int max)
    {
        this.current = current;
        this.max = max;
    }

    internal void subtract(int amount)
    {
        current -= amount;

        if(canGoNegative == false)
        {
            if(current < 0)
            {
                current = 0;
            }
        }
    }
}

public class Character : MonoBehaviour
{
    public string Name = "Nameless";
    //이동거리, A* 알고리즘을 이용하기 때문에 한칸 거리로 따지는거다.
    //한칸에 10포인트 사용한다(수평기준) 대각선은 15포인트
    //대각선을 고려한 움직임이지만 직관적이지 않다 -- 바꿔야 하나
    public float movementPoints = 50f;
    public Int2Val hp = new Int2Val(100, 100);
    public int damage = 20;
    public int attackRange = 1;
    public bool defeated;
    public void TakeDamage(int damage)
    {
        hp.subtract(damage);
        CheckDefeat();
    }

    private void CheckDefeat()
    {
        if(hp.current <= 0)
        {
            Defeated();
        }
        else
        {
            Flinch();
        }
    }

    CharacterAnimator characterAnimator;

    private void Flinch()
    {
        if(characterAnimator == null)
        {
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
        }
        characterAnimator.Flinch();
    }

    private void Defeated()
    {
        if (characterAnimator == null)
        {
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
        }
        defeated = true;
        characterAnimator.Defeated();
    }
}
