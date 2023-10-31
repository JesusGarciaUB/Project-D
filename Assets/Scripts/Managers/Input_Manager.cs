using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour
{
    private Inputs inputs;
    //singleton
    public static Input_Manager _INPUT_MANAGER;

    //timers and variables
    private float timeSinceClickPressed = 0f;
    private Vector3 mousePosition = Vector3.zero;

    private void Awake()
    {
        if (_INPUT_MANAGER != null && _INPUT_MANAGER != this) Destroy(gameObject);
        else
        {
            inputs = new Inputs();
            inputs.Character.Enable();

            //actions
            inputs.Character.Move.performed += ClickPressed;
            inputs.Character.MousePosition.performed += SetMousePosition;

            _INPUT_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        timeSinceClickPressed += Time.deltaTime;

        InputSystem.Update();
    }

    //performed functions
    private void ClickPressed(InputAction.CallbackContext context)
    {
        timeSinceClickPressed = 0f;
    }

    private void SetMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    //getters
    public bool GetClickPressed()
    {
        return timeSinceClickPressed == 0f;
    }

    public Vector3 GetMousePosition()
    {
        return mousePosition;
    }
}
