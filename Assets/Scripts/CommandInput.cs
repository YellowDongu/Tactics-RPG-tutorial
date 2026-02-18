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
    ClearUtility clearUtility;

    private void Awake()
    {
        commandManager = GetComponent<CommandManager>();
        mouseInput = GetComponent<MouseInput>();
        moveCharacter = GetComponent<MoveCharacter>();
        characterAttack = GetComponent<CharacterAttack>();
        selectCharacter = GetComponent<SelectCharacter>();
        clearUtility = GetComponent<ClearUtility>();

    }

    [SerializeField] CommandType currentCommand;
    bool isInputCommand;

    public void SetCommandType(CommandType commandType)
    {
        //커맨드 타입 전역 변수에 배정
        currentCommand = commandType;
    }

    public void InitCommand()
    {
        //커맨드 들어왔을때
        isInputCommand = true;
        //전역 변수에 있는 커맨드 타입에 따라서 케이스 실행
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
        //명령이 안내려왔을땐 휴식
        if(isInputCommand == false)
        {
            return;
        }
        //명령 상태가 변경되었으면 상태대로 간다.
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
        //공격 명령을 받아 추가 입력 받도록 대기한다.
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 클릭이 맵 내에 있을 시 진행 -- 에러나지 않게
            if (characterAttack.Check(mouseInput.positionOnGrid) == true)
            {
                //마우스 좌표의 노드에 그리드 오브젝트의 자료를 가져온다
                GridObject gridObject = characterAttack.getAttackTarget(mouseInput.positionOnGrid);
                if(gridObject == null)//그딴거 존재하지 않다면 스킵
                {
                    return;
                }
                //있으면 명령을 수행한다. 필요한 자료도 가져간다.
                commandManager.AddAttackCommand(selectCharacter.selected, mouseInput.positionOnGrid, gridObject);
                stopCommandInput();
            }
        }
        //취소
        if (Input.GetMouseButtonDown(1))
        {
            stopCommandInput();
            //하이라이트 제거
            clearUtility.ClearGridHighlightAttack();
        }
    }

    private void MoveCommandInput()
    {
        //이동 명령을 받아 추가 입력 받도록 대기한다.
        if (Input.GetMouseButtonDown(0))
        {
            if(moveCharacter.CheckOccupied(mouseInput.positionOnGrid) == true)
            {
                return;
            }
            //마우스 좌표를 주고 경로를 받는다
            List<PathNode> path = moveCharacter.GetPath(mouseInput.positionOnGrid);
            if (path == null)//경로가 없다 스킵
            {
                return;
            }
            if (path.Count == 0)
            {
                return;
            }
            //커맨드 매니저에 이동명령을 내리고 필요한 자료도 같이 준다.
            commandManager.AddMoveCommand(selectCharacter.selected, mouseInput.positionOnGrid, path);
            stopCommandInput();
        }
        //취소
        if (Input.GetMouseButtonDown(1))
        {
            stopCommandInput();
            //하이라이트 제거
            clearUtility.CleaGridHighlightMove();
            //경로 계산값 제거
            clearUtility.ClearPathfinding();
        }
    }

    private void stopCommandInput()
    {
        //선택했던 애를 선택 취소한다.
        selectCharacter.Deselect();
        //선택이 가능하게 selectcharacter update메소드를 사용 가능하게 바꾼다.
        selectCharacter.enabled = true;
        //선택중엔 취소 전까진 다른 명령으로 바꾸지 못하게(오류나지 않도록) 막음
        isInputCommand = false;
    }

    public void HighlightWalkableTerrain()
    {
        //캐릭터가 갈 수 있는 곳을 계산하고 표시해주는 메소드에게 케릭터 정보를 넘김 해당 메소드 참고
        moveCharacter.CheckWalkableTerrain(selectCharacter.selected);
    }
}
