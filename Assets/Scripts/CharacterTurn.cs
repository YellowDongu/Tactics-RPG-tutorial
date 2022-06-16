using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurn : MonoBehaviour
{
    public bool canWalk;
    public bool canAct;


    private void Start()
    {
        AddToRoundManager();
        GrantTurn();
    }

    private void AddToRoundManager()
    {
        //���� �� �ִ� CharacterTurn ��ũ��Ʈ�� ���� ������Ʈ(�� ��ũ��Ʈ)���� RoundManager���� �Ű��� ����Ʈ�� ����� �� �ְԱ� ��
        RoundManager.instance.AddMe(this);
    }

    public void GrantTurn()
    {
        //�� �Ѱ��� �� �ൿ���� �ش�.
        canWalk = true;
        canAct = true;

    }
}
