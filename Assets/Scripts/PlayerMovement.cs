using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = InputManager.Instance.MovementInput;

        gameObject.transform.position += movement * speed * Time.deltaTime;

        float moveSpeed = movement.magnitude;

        animator.SetFloat("speed", moveSpeed);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }
}
