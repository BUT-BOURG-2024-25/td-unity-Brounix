using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private int healthRestored = 1;

    [SerializeField]
    private GameObject player;
    private PlayerHealth playerHealth;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Le joueur n'a pas été trouvé ! Assurez-vous que le joueur a le tag 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(healthRestored);

                Destroy(gameObject);
            }
        }
    }
}
