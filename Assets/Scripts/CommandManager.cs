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
    //필수항목
    public Character character;
    public Vector2Int selectedGrid;
    public CommandType commandType;

    public Command(Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        //다른 메소드에서 명령 내릴때 필요한 기초 정보를 받는 애
        this.character = character;
        this.selectedGrid = selectedGrid;
        this.commandType = commandType;
    }
    //선택항목
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
        //명령이 할당되었을때 실행한다.
        if(currentCommand != null)
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        //스위치 -- 커맨드 타입에 따라서 각 케이스 중 하나 시행
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
        //명령 내릴 캐릭터 정보를 받는다
        Character receiver = currentCommand.character;
        //공격 명령을 내리고 캐릭터는 수행한다.
        receiver.GetComponent<Attack>().AttackGridObject(currentCommand.target);
        //공격 후 다른 행동 못하게 막음
        receiver.GetComponent<CharacterTurn>().canAct = false;
        //명령 초기화
        currentCommand = null;
        //잔여물이 남지 않게 청소해준다.
        clearUtility.ClearGridHighlightAttack();
    }

    private void MovementCommandExecute()
    {
        //명령 내릴 캐릭터 정보를 받는다
        Character receiver = currentCommand.character;
        //이동 명령을 내리고 캐릭터는 수행한다.
        receiver.GetComponent<Movement>().Move(currentCommand.path);
        //공격 후 다시 움직이지 못하게 막음
        receiver.GetComponent<CharacterTurn>().canWalk = false;
        //다음 명령을 내리기 위해 + 대기 상태로 만들기 위해 명령 초기화
        currentCommand = null;
        //잔여물이 남지 않게 청소해준다.
        clearUtility.ClearPathfinding();
        clearUtility.CleaGridHighlightMove();
    }

    public void AddMoveCommand(Character character, Vector2Int selectedGrid, List<PathNode> path)
    {
        //외부에서 커맨드를 받은 걸 이 스크립트로 들여온다. + 필요한 정보도 같이 불러온다.
        currentCommand = new Command(character, selectedGrid, CommandType.MoveTo);
        currentCommand.path = path;
    }

    internal void AddAttackCommand(Character attacker, Vector2Int selectGrid, GridObject target)
    {
        //외부에서 커맨드를 받은 걸 이 스크립트로 들여온다. + 필요한 정보도 같이 불러온다.
        currentCommand = new Command(attacker, selectGrid, CommandType.Attack);
        currentCommand.target = target;
    }


    /* 기본형 저장
    public void AddCommand(Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        currentCommand = new Command(character, selectedGrid, commandType);
    }
    */
}
