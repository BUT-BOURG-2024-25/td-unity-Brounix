using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementPhysic : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float JumpPower = 5f;
    [SerializeField]
    private Rigidbody physicsBody;

    private Animator animator;
    private bool isGrounded;

    private Camera mainCamera;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        if (physicsBody == null)
        {
            physicsBody = GetComponent<Rigidbody>();
        }

        mainCamera = Camera.main;

        InputManager.Instance.RegisterOnJumpInput(Jump, true);
    }

    private void OnDestroy() 
    {
        InputManager.Instance.RegisterOnJumpInput(Jump, false);
    }

    private void Update()
    {
        Vector3 movementInput = InputManager.Instance.MovementInput;

        // Créer un vecteur de mouvement basé sur la direction de la caméra
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0; // Ignorer la direction verticale
        forward.Normalize(); // Normaliser le vecteur

        // Créer un vecteur de droite pour la direction de déplacement
        Vector3 right = mainCamera.transform.right;

        // Calculer la direction du mouvement en inversant la direction lorsque le joueur veut aller en arrière
        Vector3 movement = (forward * movementInput.z + right * movementInput.x).normalized;

        // Appliquer la vitesse au corps physique
        physicsBody.velocity = new Vector3(movement.x * speed, physicsBody.velocity.y, movement.z * speed);

        float moveSpeed = movement.magnitude;

        animator.SetFloat("speed", moveSpeed);
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        animator.SetBool("isGrounded", isGrounded);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (isGrounded)
        {
            physicsBody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }
}
