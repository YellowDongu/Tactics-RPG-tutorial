using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    GridObject gridObject;
    CharacterAnimator characterAnimator;

    List<Vector3> pathWorldPositions;

    public bool IS_MOVING
    {
        get
        {
            if(pathWorldPositions == null)
            {
                return false;
            }
            return pathWorldPositions.Count > 0;
        }
    }

    [SerializeField] float moveSpeed = 1f;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
    }

    public void Move(List<PathNode> path)
    {
        if (IS_MOVING)
        {
            SkipAnimation();
        }

        pathWorldPositions =  gridObject.targetGrid.ConvertPathNodesToWorldPositions(path);

        gridObject.targetGrid.RemoveObject(gridObject.positionOnGrid, gridObject);

        gridObject.positionOnGrid.x = path[path.Count - 1].pos_x;
        gridObject.positionOnGrid.y = path[path.Count - 1].pos_y;

        gridObject.targetGrid.PlaceObject(gridObject.positionOnGrid, gridObject);

        RotateCharacter(transform.position, pathWorldPositions[0]);

        characterAnimator.StartMoving();
    }


    private void RotateCharacter(Vector3 originPosition, Vector3 destinationPosition)
    {
        Vector3 direction = (destinationPosition - originPosition).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        if(pathWorldPositions == null)
        {
            return;
        }
        if(pathWorldPositions.Count == 0)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, pathWorldPositions[0], moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, pathWorldPositions[0]) < 0.05f)
        {
            pathWorldPositions.RemoveAt(0);
            if(pathWorldPositions.Count == 0)
            {
                characterAnimator.StopMoving();
            }
            else
            {
                RotateCharacter(transform.position, pathWorldPositions[0]);
            }
        }
    }

    public void SkipAnimation()
    {
        if(pathWorldPositions.Count < 2)
        {
            return;
        }
        transform.position = pathWorldPositions[pathWorldPositions.Count - 1];
        Vector3 originPosition = pathWorldPositions[pathWorldPositions.Count - 2];
        Vector3 destinationPosition = pathWorldPositions[pathWorldPositions.Count - 1];
        RotateCharacter(originPosition, destinationPosition);
        pathWorldPositions.Clear();
        characterAnimator.StopMoving();
    }
}
