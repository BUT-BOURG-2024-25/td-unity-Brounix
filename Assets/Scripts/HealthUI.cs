using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (heartImages == null || heartImages.Length != maxHealth)
        {
            Debug.LogError("Health UI not properly set up!");
            return;
        }

        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}
