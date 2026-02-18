using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Allegiance
{
    Player,
    Opponent
}

public class CharacterTurn : MonoBehaviour
{
    public Allegiance allegiance;
    public bool canWalk;
    public bool canAct;


    private void Start()
    {
        AddToRoundManager();
        GrantTurn();
    }

    private void AddToRoundManager()
    {
        //월드 상에 있는 CharacterTurn 스크립트를 가진 오브젝트(의 스크립트)들이 RoundManager에게 신고해 리스트에 등록할 수 있게금 함
        RoundManager.instance.AddMe(this);
    }

    public void GrantTurn()
    {
        //턴 넘겼을 떄 행동권을 준다.
        canWalk = true;
        canAct = true;

    }
}
