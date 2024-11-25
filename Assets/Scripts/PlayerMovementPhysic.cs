using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovementPhysic : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float rotationSpeed = 200f;
    [SerializeField]
    private float jumpPower = 5f;
    
    private Rigidbody physicsBody;
    private Animator animator;
    private bool isGrounded;
    private Camera mainCamera;

    [SerializeField]
    private float sprintSpeed = 10f;
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float staminaDrainRate = 10f;
    [SerializeField]
    private float staminaRecoveryRate = 5f;

    private float currentStamina;
    private bool isSprinting = false;

    [SerializeField] 
    private Slider staminaBar;

    private float mouseX;

    [SerializeField]
    private bool useJoystick = false;

    [SerializeField]
    private Button jumpButton;

    [SerializeField] 
    private Button sprintButton;

    private void Start()
    {
        jumpButton.onClick.AddListener(OnJumpButtonClicked);
        sprintButton.onClick.AddListener(OnSprintButtonClicked);

        animator = GetComponent<Animator>();
        physicsBody = physicsBody ?? GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }

        InputManager.Instance.RegisterOnJumpInput(Jump, true);
        InputManager.Instance.RegisterOnSprintInput(StartSprinting, true);
    }

    private void OnDestroy() 
    {
        InputManager.Instance.RegisterOnJumpInput(Jump, false);
        InputManager.Instance.RegisterOnSprintInput(StartSprinting, false);
    }

    private void Update()
    {
        Vector3 movementInput = Vector3.zero;
        if (useJoystick)
        {
            movementInput = new Vector3(UIManager.Instance.Joystick.Direction.x, 0.0f, UIManager.Instance.Joystick.Direction.y);
        }
        else
        {
            movementInput = InputManager.Instance.MovementInput;
        }
        
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = mainCamera.transform.right;

        Vector3 movement = (forward * movementInput.z + right * movementInput.x).normalized;

        float moveSpeed = movement.magnitude;

        if (InputManager.Instance.GetSprintAction().action.ReadValue<float>() > 0 && currentStamina > 0 && isGrounded && moveSpeed > 0.1f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        float appliedSpeed = isSprinting && currentStamina > 0 ? sprintSpeed : speed;
        physicsBody.velocity = new Vector3(movement.x * appliedSpeed, physicsBody.velocity.y, movement.z * appliedSpeed);

        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0)
            {
                isSprinting = false; 
            }
        }
        else if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }
        animator.SetFloat("speed", moveSpeed);
        animator.SetBool("isSprinting", isSprinting);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        animator.SetBool("isGrounded", isGrounded);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }



    private void StartSprinting(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            isSprinting = context.ReadValue<float>() > 0 && currentStamina > 0;
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (isGrounded)
        {
            physicsBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    private void OnJumpButtonClicked()
    {
        if (isGrounded)
        {
            physicsBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    private void OnSprintButtonClicked()
    {
        if (isGrounded)
        {
            isSprinting = true;
        }
    }
}
