using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float changeDirectionTime = 2f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Vector3 movementDirection;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        SetRandomMovementDirection();
        StartCoroutine(ChangeDirection());
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);

        float moveSpeed = movementDirection.magnitude;
        animator.SetFloat("slime", moveSpeed);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void SetRandomMovementDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        movementDirection = new Vector3(randomX, 0, randomZ).normalized;
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionTime);
            SetRandomMovementDirection();
        }
    }
}
