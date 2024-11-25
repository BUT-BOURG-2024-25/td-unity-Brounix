using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public HealthUI healthUI;
    private Animator animator;
    public GameObject gameOverScreen;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }

        if (currentHealth == 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetBool("Die", true);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        animator.SetBool("Die", false);
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        currentHealth = maxHealth;
        healthUI.UpdateHealthUI(currentHealth, maxHealth);

        GameManager.Instance.GameOver();
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}
