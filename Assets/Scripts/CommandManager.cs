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
        this.character = character;
        this.selectedGrid = selectedGrid;
        this.commandType = commandType;
    }

    public List<PathNode> path;
    public GridObject target;
}

public class CommandManager : MonoBehaviour
{
    Command currentCommand;

    CommandInput commandInput;

    private void Start()
    {
        commandInput = GetComponent<CommandInput>();

    }

    private void Update()
    {
        if(currentCommand != null)
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        //MovementCommandExecute();
        AttackCommandExecute();
    }

    private void AttackCommandExecute()
    {
        Character receiver = currentCommand.character;
        receiver.GetComponent<Attack>().AttackPosition(currentCommand.target);
        currentCommand = null;
    }

    private void MovementCommandExecute()
    {
        Character receiver = currentCommand.character;
        receiver.GetComponent<Movement>().Move(currentCommand.path);
        currentCommand = null;
        commandInput.HighlightWalkableTerrain();
    }

    public void AddMoveCommand(Character character, Vector2Int selectedGrid, List<PathNode> path)
    {
        currentCommand = new Command(character, selectedGrid, CommandType.MoveTo);
        currentCommand.path = path;
    }

    internal void AddAttackCommand(Character attacker, Vector2Int selectGrid, GridObject target)
    {
        currentCommand = new Command(attacker, selectGrid, CommandType.Attack);
        currentCommand.target = target;
    }


    /*
    public void AddCommand(Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        currentCommand = new Command(character, selectedGrid, commandType);
    }
    */
}
