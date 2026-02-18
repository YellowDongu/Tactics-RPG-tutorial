using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //싱글턴 패턴
    //static - 실행 후 해당 클래스가 처음으로 사용될 때 한번 초기화, 계속 동일한 메모리를 사용
    public static RoundManager instance;

    private void Awake()
    {
        //인스턴스를 자신으로 지정
        instance = this;
    }

    //싱글턴 패턴 끝


    [SerializeField] ForceContainer playerForceContainer;
    [SerializeField] ForceContainer opponentForceContainer;
    //시작시 1번째 턴
    int round = 1;
    //턴을 표시할 텍스트(TMP) 지정
    [SerializeField] TMPro.TextMeshProUGUI turnCountText;
    [SerializeField] TMPro.TextMeshProUGUI forceRoundText;

    private void Start()
    {
        //시작할떄 텍스트 수정
        UpdateTextOnScreen();
    }

    public void AddMe(CharacterTurn character)
    {
        if(character.allegiance == Allegiance.Player)
        {
            playerForceContainer.AddMe(character);
        }
        if(character.allegiance == Allegiance.Opponent)
        {
            opponentForceContainer.AddMe(character);
        }
    }

    Allegiance currentTurn;

    public void NextTurn()
    {
        switch (currentTurn)
        {
            case Allegiance.Player:
                currentTurn = Allegiance.Opponent;
                break;
            case Allegiance.Opponent:
                NextRound();
                currentTurn = Allegiance.Player;
                break;
        }

        GrantTurnToForce();
        //텍스트 수정(TMP TEXT도 포함)
        UpdateTextOnScreen();
    }

    private void GrantTurnToForce()
    {
        switch (currentTurn)
        {
            case Allegiance.Player:
                playerForceContainer.GrantTurn();
                break;
            case Allegiance.Opponent:
                opponentForceContainer.GrantTurn();
                break;
        }
    }

    public void NextRound()
    {
        //턴 넘기기
        //이 변수로 몇번째 턴인지 저장
        round += 1;
    }

    void UpdateTextOnScreen()
    {
        //텍스트 표시 -- 턴 표시
        turnCountText.text = "Turn : " + round.ToString();
        forceRoundText.text = currentTurn.ToString();
    }
}
