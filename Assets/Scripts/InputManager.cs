using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputActionReference MovementAction = null;
    [SerializeField]
    private InputActionReference JumpAction = null;
    [SerializeField]
    private InputActionReference ClickAction = null;
    [SerializeField]
    private InputActionReference SprintAction = null;
    
    public static InputManager Instance {get {return _instance;}}
    private static InputManager _instance = null;

    public Vector3 MovementInput {get; private set;} 

    public Action<Finger> FingerDownAction = null;

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

    public void RegisterOnSprintInput(Action<InputAction.CallbackContext> OnSprintAction, bool register)
    {
        if(register)
            SprintAction.action.performed += OnSprintAction;
        else
            SprintAction.action.performed -= OnSprintAction;
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

    public InputActionReference GetSprintAction()
    {
        return SprintAction;
    }

    private void Update()
    {
        Vector2 MoveInput = MovementAction.action.ReadValue<Vector2>();
        MovementInput = new Vector3(MoveInput.x, 0, MoveInput.y);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }
    
    private void OnFingerDown(Finger finger)
    {
        Vector2 screenPosTouch = finger.screenPosition;
        RectTransform joystickRect = UIManager.Instance.Joystick.transform as RectTransform;

        bool isInX = joystickRect.offsetMin.x <= screenPosTouch.x && screenPosTouch.x <= joystickRect.offsetMax.x;
        bool isInY = joystickRect.offsetMin.y <= screenPosTouch.y && screenPosTouch.y <= joystickRect.offsetMax.y;

        if (!isInX || !isInY)
            FingerDownAction?.Invoke(finger);    
    }
}
