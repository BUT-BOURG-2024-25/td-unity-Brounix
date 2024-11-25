using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float staminaDrainRate = 10f;
    [SerializeField]
    private float staminaRecoveryRate = 5f;
    [SerializeField]
    private Slider staminaSlider;

    private float currentStamina;
    private bool isSprinting;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    private void Update()
    {
        if (isSprinting && currentStamina > 0)
        {
            DrainStamina();
        }
        else if (!isSprinting && currentStamina < maxStamina)
        {
            RecoverStamina();
        }

        UpdateStaminaUI();
    }

    public void SetSprinting(bool sprinting)
    {
        isSprinting = sprinting && currentStamina > 0;
    }

    private void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            isSprinting = false;
        }
    }

    private void RecoverStamina()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }
}
