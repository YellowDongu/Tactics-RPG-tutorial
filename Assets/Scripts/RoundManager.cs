using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //�̱��� ����
    //static - ���� �� �ش� Ŭ������ ó������ ���� �� �ѹ� �ʱ�ȭ, ��� ������ �޸𸮸� ���
    public static RoundManager instance;

    private void Awake()
    {
        //�ν��Ͻ��� �ڽ����� ����
        instance = this;
    }

    //�̱��� ���� ��


    [SerializeField] ForceContainer playerForceContainer;
    [SerializeField] ForceContainer opponentForceContainer;
    //���۽� 1��° ��
    int round = 1;
    //���� ǥ���� �ؽ�Ʈ(TMP) ����
    [SerializeField] TMPro.TextMeshProUGUI turnCountText;
    [SerializeField] TMPro.TextMeshProUGUI forceRoundText;

    private void Start()
    {
        //�����ҋ� �ؽ�Ʈ ����
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
        //�ؽ�Ʈ ����(TMP TEXT�� ����)
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
        //�� �ѱ��
        //�� ������ ���° ������ ����
        round += 1;
    }

    void UpdateTextOnScreen()
    {
        //�ؽ�Ʈ ǥ�� -- �� ǥ��
        turnCountText.text = "Turn : " + round.ToString();
        forceRoundText.text = currentTurn.ToString();
    }
}
