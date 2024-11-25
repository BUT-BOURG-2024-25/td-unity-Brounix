using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject attackTrigger;
    [SerializeField]
    private GameObject slashEffectPrefab;
    [SerializeField]
    private GameObject enemyHitEffectPrefab; 

    private bool isAttacking = false;

    private void Start()
    {
        if (attackTrigger != null)
        {
            attackTrigger.SetActive(false);
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator is missing on the player. Please assign an Animator component.");
            }
        }

        InputManager.Instance.RegisterOnClickInput(Attack, true);
        InputManager.Instance.FingerDownAction += OnFingerDown;

    }

    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnClickInput(Attack, false);
        InputManager.Instance.FingerDownAction -= OnFingerDown;
    }

    private void OnFingerDown(Finger finger)
    {
        PerformAttack();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        PerformAttack();
    }


    private void PerformAttack()
    {
        if (isAttacking) return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (slashEffectPrefab != null)
        {
            InstantiateSlashEffect();
        }

        StartCoroutine(ActivateAttackTrigger());
    }

    private IEnumerator ActivateAttackTrigger()
    {
        attackTrigger.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackTrigger.SetActive(false); 
        yield return new WaitForSeconds(1f);
        isAttacking = false; 
    }

    private void InstantiateSlashEffect()
    {
        GameObject slashEffect = Instantiate(slashEffectPrefab, transform.position, transform.rotation);

        slashEffect.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        slashEffect.transform.position = new Vector3(slashEffect.transform.position.x, slashEffect.transform.position.y + 0.6f, slashEffect.transform.position.z);

        Destroy(slashEffect, 0.5f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;

            if (GameManager.Instance != null && GameManager.Instance.enemies.Contains(enemy))
            {
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.TakeDamage(1);

                    if (enemyHitEffectPrefab != null)
                    {
                        InstantiateEnemyHitEffect(other.transform.position);
                    }
                }
            }
        }
    }


    private void InstantiateEnemyHitEffect(Vector3 enemyPosition)
    {
        GameObject hitEffect = Instantiate(enemyHitEffectPrefab, enemyPosition, Quaternion.identity);
        hitEffect.transform.position = new Vector3(hitEffect.transform.position.x, hitEffect.transform.position.y + 0.3f, hitEffect.transform.position.z);
        Destroy(hitEffect, 0.5f);
    }
}
