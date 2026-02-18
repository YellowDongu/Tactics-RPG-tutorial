using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    //캐릭터 애니메이터 건들여주는 애 설명은 생략한다.
    Animator animator;

    [SerializeField] bool move;
    [SerializeField] bool attack;
    [SerializeField] bool defeated;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    public void StartMoving()
    {
        move = true;
    }
    public void StopMoving()
    {
        move = false;
    }

    public void Defeated()
    {
        defeated = true;
    }

    public void Attack()
    {
        attack = true;
    }

    private void LateUpdate()
    {
        animator.SetBool("Move", move);
        animator.SetBool("Attack", attack);
        animator.SetBool("Defeated", defeated);
        if (attack == true)
        {
            attack = false;
        }
    }

    internal void Flinch()
    {
        animator.SetTrigger("Pain");
    }
}
