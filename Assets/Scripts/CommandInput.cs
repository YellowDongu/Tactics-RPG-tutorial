using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour
{
    CommandManager commandManager;
    MouseInput mouseInput;
    MoveCharacter moveCharacter;


    private void Awake()
    {
        commandManager = GetComponent<CommandManager>();
        mouseInput = GetComponent<MouseInput>();
        moveCharacter = GetComponent<MoveCharacter>();
    }

    [SerializeField] Character selectedCharacter;
    [SerializeField] CommandType currentCommand;

    private void Start()
    {
        HighlightWalkableTerrain();
    }

    public void HighlightWalkableTerrain()
    {
        moveCharacter.CheckWalkableTerrain(selectedCharacter);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<PathNode> path = moveCharacter.GetPath(mouseInput.positionOnGrid);
            commandManager.AddMoveCommand(selectedCharacter, mouseInput.positionOnGrid, path);
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedCharacter.GetComponent<Movement>().SkipAnimation();
        }
    }
}
