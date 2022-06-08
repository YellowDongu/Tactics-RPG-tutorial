using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour
{
    CommandManager commandManager;
    MouseInput mouseInput;
    MoveCharacter moveCharacter;
    CharacterAttack characterAttack;
    SelectCharacter selectCharacter;

    private void Awake()
    {
        commandManager = GetComponent<CommandManager>();
        mouseInput = GetComponent<MouseInput>();
        moveCharacter = GetComponent<MoveCharacter>();
        characterAttack = GetComponent<CharacterAttack>();
        selectCharacter = GetComponent<SelectCharacter>();
    }

    [SerializeField] CommandType currentCommand;
    bool isInputCommand;

    public void SetCommandType(CommandType commandType)
    {
        //Ŀ�ǵ� Ÿ�� ���� ������ ����
        currentCommand = commandType;
    }

    public void InitCommand()
    {
        //Ŀ�ǵ� ��������
        isInputCommand = true;
        //���� ������ �ִ� Ŀ�ǵ� Ÿ�Կ� ���� ���̽� ����
        switch (currentCommand)
        {
            case CommandType.MoveTo:
                HighlightWalkableTerrain();
                break;
            case CommandType.Attack:
                characterAttack.CalculateAttackArea(selectCharacter.selected.GetComponent<GridObject>().positionOnGrid, selectCharacter.selected.attackRange);
                break;
        }
    }

    private void Update()
    {
        //����� �ȳ��������� �޽�
        if(isInputCommand == false)
        {
            return;
        }
        //��� ���°� ����Ǿ����� ���´�� ����.
        switch (currentCommand)
        {
            case CommandType.MoveTo:
                MoveCommandInput();
                break;
            case CommandType.Attack:
                AttackCommandInput();
                break;
        }
    }

    private void AttackCommandInput()
    {
        //���� ����� �޾� �߰� �Է� �޵��� ����Ѵ�.
        if (Input.GetMouseButtonDown(0))
        {
            //���콺 Ŭ���� �� ���� ���� �� ���� -- �������� �ʰ�
            if (characterAttack.Check(mouseInput.positionOnGrid) == true)
            {
                //���콺 ��ǥ�� ��忡 �׸��� ������Ʈ�� �ڷḦ �����´�
                GridObject gridObject = characterAttack.getAttackTarget(mouseInput.positionOnGrid);
                if(gridObject == null)//�׵��� �������� �ʴٸ� ��ŵ
                {
                    return;
                }
                //������ ����� �����Ѵ�. �ʿ��� �ڷᵵ ��������.
                commandManager.AddAttackCommand(selectCharacter.selected, mouseInput.positionOnGrid, gridObject);
                //���õ� �ָ� ���� �����Ѵ�.
                selectCharacter.Deselect();
                //������ �����ϰ� selectcharacter update�޼ҵ带 ��� �����ϰ� �ٲ۴�.
                selectCharacter.enabled = true;
            }
        }
    }

    private void MoveCommandInput()
    {
        //�̵� ����� �޾� �߰� �Է� �޵��� ����Ѵ�.
        if (Input.GetMouseButtonDown(0))
        {
            //���콺 ��ǥ�� �ְ� ��θ� �޴´�
            List<PathNode> path = moveCharacter.GetPath(mouseInput.positionOnGrid);
            if (path == null)//��ΰ� ���� ��ŵ
            {
                return;
            }
            if (path.Count == 0)
            {
                return;
            }
            //Ŀ�ǵ� �Ŵ����� �̵������ ������ �ʿ��� �ڷᵵ ���� �ش�.
            commandManager.AddMoveCommand(selectCharacter.selected, mouseInput.positionOnGrid, path);
            //�����ߴ� �ָ� ���� ����Ѵ�.
            selectCharacter.Deselect();
            //������ �����ϰ� selectcharacter update�޼ҵ带 ��� �����ϰ� �ٲ۴�.
            selectCharacter.enabled = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            //selectCharacter.selected.GetComponent<Movement>().SkipAnimation();
        }
    }

    public void HighlightWalkableTerrain()
    {
        //ĳ���Ͱ� �� �� �ִ� ���� ����ϰ� ǥ�����ִ� �޼ҵ忡�� �ɸ��� ������ �ѱ� �ش� �޼ҵ� ����
        moveCharacter.CheckWalkableTerrain(selectCharacter.selected);
    }
}
