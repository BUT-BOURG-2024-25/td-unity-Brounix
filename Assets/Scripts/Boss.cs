using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boss : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private int health = 10;
    [SerializeField]
    private float attackCooldown = 3f;
    [SerializeField]
    private float aoeCooldown = 5f;
    [SerializeField]
    private int aoeDamage = 2;

    private Transform player;
    private Animator animator;
    private Rigidbody rb;
    private bool isDead = false;
    private bool isAttacking = false;
    private float attackTimer;
    private float aoeTimer;

    [SerializeField]
    private GameObject basicAttackZone;
    [SerializeField]
    private GameObject aoeAttackZone;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (basicAttackZone == null || aoeAttackZone == null || player == null || animator == null || rb == null)
        {
            Debug.LogError("Assurez-vous que toutes les références sont bien assignées dans l'inspecteur.");
            enabled = false;
        }

        basicAttackZone.SetActive(false);
        aoeAttackZone.SetActive(false);

        attackTimer = 0f;
        aoeTimer = 0f;
    }

    private void Update()
    {
        if (isDead) return;

        attackTimer -= Time.deltaTime;
        aoeTimer -= Time.deltaTime;

        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 movementDirection = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float stoppingDistance = 4.5f;

        if (distanceToPlayer > stoppingDistance)
        {
            rb.MovePosition(rb.position + movementDirection * speed * Time.deltaTime);
            animator.SetFloat("BossWalk", speed);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
        else
        {
            animator.SetFloat("BossWalk", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead || isAttacking) return;

        if (other.CompareTag("Player"))
        {
            if (attackTimer <= 0f)
            {
                StartCoroutine(BasicAttack());
            }
            else if (aoeTimer <= 0f)
            {
                StartCoroutine(AOEAttack());
            }
        }
    }

    private IEnumerator BasicAttack()
    {
        isAttacking = true;
        animator.SetTrigger("BasicAttack");
        basicAttackZone.SetActive(true);
        yield return new WaitForSeconds(1f);
        basicAttackZone.SetActive(false);
        attackTimer = attackCooldown;
        isAttacking = false;
    }

    private IEnumerator AOEAttack()
    {
        isAttacking = true;
        animator.SetTrigger("AOEAttack");
        aoeAttackZone.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        aoeAttackZone.SetActive(false);
        aoeTimer = aoeCooldown;
        isAttacking = false;
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

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("BossDeath");
        rb.isKinematic = true;
        Destroy(gameObject, 3f);
    }
}
