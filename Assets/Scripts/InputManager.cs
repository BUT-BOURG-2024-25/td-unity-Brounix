using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference MovementAction = null;
    [SerializeField]
    private InputActionReference JumpAction = null;
    [SerializeField]
    private InputActionReference ClickAction = null;
    
    public static InputManager Instance {get {return _instance;}}
    private static InputManager _instance = null;

    public Vector3 MovementInput {get; private set;} 

    public void RegisterOnJumpInput(Action<InputAction.CallbackContext> OnJumpAction, bool register)
    {
        if(register)
            JumpAction.action.performed += OnJumpAction;
        else
            JumpAction.action.performed -= OnJumpAction;
    }

    public void RegisterOnClickInput(Action<InputAction.CallbackContext> OnClickAction, bool register)
    {
        if(register)
            ClickAction.action.performed += OnClickAction;
        else
            ClickAction.action.performed -= OnClickAction;
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        } else 
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector2 MoveInput = MovementAction.action.ReadValue<Vector2>();
        MovementInput = new Vector3(MoveInput.x, 0, MoveInput.y);
    }
}
