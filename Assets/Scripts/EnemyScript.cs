using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float deathDelay = 2.5f;

    private Vector3 movementDirection;
    private Rigidbody rb;
    private Animator animator;
    private bool isDead = false;

    private Transform player;
    private PlayerHealth playerHealth;

    [SerializeField]
    private BoxCollider attackCollider;
    
    [SerializeField]
    private int health = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        playerHealth = player.GetComponent<PlayerHealth>();

        if (attackCollider == null)
        {
            Debug.LogError("Le BoxCollider pour la détection de collision n'est pas assigné!");
        }
        else
        {
            attackCollider.isTrigger = true;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (player != null)
        {
            movementDirection = (player.position - transform.position).normalized;
        }

        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);

        float moveSpeed = movementDirection.magnitude;
        animator.SetFloat("EnemyWalk", moveSpeed);

        if (moveSpeed > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("EnemyDeath");
        rb.isKinematic = true;
        GameManager.Instance.RemoveEnemyFromList(gameObject);
        Destroy(gameObject, deathDelay);
    }
}
