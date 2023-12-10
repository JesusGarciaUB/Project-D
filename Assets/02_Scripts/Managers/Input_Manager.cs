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
    private float timeSinceCancelPressed = 0f;
    private float timeSinceQPressed = 0f;
    private float timeSinceWPressed = 0f;
    private float timeSinceEPressed = 0f;
    private float timeSinceRPressed = 0f;
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
            inputs.Character.Cancel.performed += CancelPressed;
            inputs.Character.Q.performed += QPressed;
            inputs.Character.W.performed += WPressed;
            inputs.Character.E.performed += EPressed;
            inputs.Character.R.performed += RPressed;

            _INPUT_MANAGER = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        timeSinceClickPressed += Time.deltaTime;
        timeSinceCancelPressed += Time.deltaTime;
        timeSinceQPressed += Time.deltaTime;
        timeSinceWPressed += Time.deltaTime;
        timeSinceEPressed += Time.deltaTime;
        timeSinceRPressed += Time.deltaTime;

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

    private void CancelPressed(InputAction.CallbackContext context)
    {
        timeSinceCancelPressed = 0f;
    }

    private void QPressed(InputAction.CallbackContext context)
    {
        timeSinceQPressed = 0f;
    }

    private void WPressed(InputAction.CallbackContext context)
    {
        timeSinceWPressed = 0f;
    }

    private void EPressed(InputAction.CallbackContext context)
    {
        timeSinceEPressed = 0f;
    }

    private void RPressed(InputAction.CallbackContext context)
    {
        timeSinceRPressed = 0f;
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

    public bool GetCancelPressed()
    {
        return timeSinceCancelPressed == 0f;
    }
    public bool GetQPressed()
    {
        return timeSinceQPressed == 0f;
    }
    public bool GetWPressed()
    {
        return timeSinceWPressed == 0f;
    }
    public bool GetEPressed()
    {
        return timeSinceEPressed == 0f;
    }
    public bool GetRPressed()
    {
        return timeSinceRPressed == 0f;
    }
}
