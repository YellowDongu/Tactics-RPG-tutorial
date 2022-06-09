using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    MoveTo,
    Attack
}

public class Command
{
    public Character character;
    public Vector2Int selectedGrid;
    public CommandType commandType;

    public Command(Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        //�ٸ� �޼ҵ忡�� ��� ������ �ʿ��� ���� ������ �޴� ��
        this.character = character;
        this.selectedGrid = selectedGrid;
        this.commandType = commandType;
    }

    public List<PathNode> path;
    public GridObject target;
}

public class CommandManager : MonoBehaviour
{
    ClearUtility clearUtility;

    private void Awake()
    {
        clearUtility = GetComponent<ClearUtility>();
    }

    Command currentCommand;

    CommandInput commandInput;

    private void Start()
    {
        commandInput = GetComponent<CommandInput>();

    }

    private void Update()
    {
        //����� �Ҵ�Ǿ����� �����Ѵ�.
        if(currentCommand != null)
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        //����ġ -- Ŀ�ǵ� Ÿ�Կ� ���� �� ���̽� �� �ϳ� ����
        switch (currentCommand.commandType)
        {
            case CommandType.MoveTo:
                MovementCommandExecute();
                break;
            case CommandType.Attack:
                AttackCommandExecute();
                break;
        }
    }

    private void AttackCommandExecute()
    {
        //��� ���� ĳ���� ������ �޴´�
        Character receiver = currentCommand.character;
        //���� ����� ������ ĳ���ʹ� �����Ѵ�.
        receiver.GetComponent<Attack>().AttackPosition(currentCommand.target);
        //���� �� �ٸ� �ൿ ���ϰ� ����
        receiver.GetComponent<CharacterTurn>().canAct = false;
        //��� �ʱ�ȭ
        currentCommand = null;
        //�ܿ����� ���� �ʰ� û�����ش�.
        clearUtility.ClearGridHighlightAttack();
    }

    private void MovementCommandExecute()
    {
        //��� ���� ĳ���� ������ �޴´�
        Character receiver = currentCommand.character;
        //�̵� ����� ������ ĳ���ʹ� �����Ѵ�.
        receiver.GetComponent<Movement>().Move(currentCommand.path);
        //���� �� �ٽ� �������� ���ϰ� ����
        receiver.GetComponent<CharacterTurn>().canWalk = false;
        //���� ����� ������ ���� + ��� ���·� ����� ���� ��� �ʱ�ȭ
        currentCommand = null;
        //�ܿ����� ���� �ʰ� û�����ش�.
        clearUtility.ClearPathfinding();
        clearUtility.CleaGridHighlightMove();
    }

    public void AddMoveCommand(Character character, Vector2Int selectedGrid, List<PathNode> path)
    {
        //�ܺο��� Ŀ�ǵ带 ���� �� �� ��ũ��Ʈ�� �鿩�´�. + �ʿ��� ������ ���� �ҷ��´�.
        currentCommand = new Command(character, selectedGrid, CommandType.MoveTo);
        currentCommand.path = path;
    }

    internal void AddAttackCommand(Character attacker, Vector2Int selectGrid, GridObject target)
    {
        //�ܺο��� Ŀ�ǵ带 ���� �� �� ��ũ��Ʈ�� �鿩�´�. + �ʿ��� ������ ���� �ҷ��´�.
        currentCommand = new Command(attacker, selectGrid, CommandType.Attack);
        currentCommand.target = target;
    }


    /* �⺻�� ����
    public void AddCommand(Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        currentCommand = new Command(character, selectedGrid, commandType);
    }
    */
}
