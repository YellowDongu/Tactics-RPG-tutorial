using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float KeyboardInputSensitivity = 1f;
    [SerializeField] float MouseInputSensivity = 1f;

    [SerializeField] bool continious = true;

    [SerializeField] Transform bottomLeftBoarder;
    [SerializeField] Transform topRightBoarder;

    Vector3 input;
    Vector3 pointOfOrigin;




    void Update()
    {
        NullInput();

        MoveCameraInput();

        MoveCamera();
    }

    private void NullInput()
    {
        input.x = 0;
        input.y = 0;
        input.z = 0;
    }

    private void MoveCamera()
    {
        Vector3 position = transform.position;
        position += (input * Time.deltaTime);
        position.x = Mathf.Clamp(position.x, bottomLeftBoarder.position.x, topRightBoarder.position.x);
        position.z = Mathf.Clamp(position.z, bottomLeftBoarder.position.z, topRightBoarder.position.z);

        transform.position = position;
    }

    private void MoveCameraInput()
    {
        AxisInput();
        MouseInput();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointOfOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseInput = Input.mousePosition;
            input.x += (mouseInput.x - pointOfOrigin.x) * MouseInputSensivity;
            input.z += (mouseInput.y - pointOfOrigin.y) * MouseInputSensivity;
            if(continious == false)
            {
                pointOfOrigin = mouseInput;
            }
        }
    }

    private void AxisInput()
    {
        input.x += Input.GetAxisRaw("Horizontal") * KeyboardInputSensitivity;
        input.z += Input.GetAxisRaw("Vertical") * KeyboardInputSensitivity;
    }
}
