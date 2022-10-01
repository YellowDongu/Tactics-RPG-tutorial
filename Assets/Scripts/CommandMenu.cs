using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject moveButton;
    [SerializeField] GameObject attackButton;
    CommandInput commandInput;

    SelectCharacter selectCharacter;

    private void Awake()
    {
        commandInput = GetComponent<CommandInput>();
        selectCharacter = GetComponent<SelectCharacter>();
    }

    public void OpenPanel(CharacterTurn characterTurn)
    {
        selectCharacter.enabled = false;
        panel.SetActive(true);

        if (characterTurn.allegiance != Allegiance.Player)
        {
            moveButton.SetActive(false);
            attackButton.SetActive(false);

        }
        else
        {
            if (characterTurn.canAct)
            {
                attackButton.SetActive(true);
            }
            else
            {
                attackButton.SetActive(false);
            }

            if (characterTurn.canWalk)
            {
                moveButton.SetActive(true);
            }
            else
            {
                moveButton.SetActive(false);
            }
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false);

    }

    public void MoveCommandSelected()
    {
        //턴이 왔는지 확인
        if (selectCharacter.selected.GetComponent<CharacterTurn>().canWalk)
        {
            commandInput.SetCommandType(CommandType.MoveTo);
            commandInput.InitCommand();
            ClosePanel();
        }
    }

    public void AttackCommandSelected()
    {
        //턴이 왔는지 확인
        if (selectCharacter.selected.GetComponent<CharacterTurn>().canAct)
        {
            commandInput.SetCommandType(CommandType.Attack);
            commandInput.InitCommand();
            ClosePanel();
        }
    }
}
