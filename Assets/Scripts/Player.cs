using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField]
    private Slider staminaBar;
    public float maxStamina;
    public float currentStamina;
    public float staminaRegenRate;
    public float staminaRegenDelay;
    private bool isDrainingStamina;
    private float regenCooldownTimer = 0f;

    [Header("Health Settings")]
    [SerializeField]
    private Slider healthBar;
    public float maxHealth;
    public float currentHealth;

    public float strenght;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;

        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth;
        staminaBar.value = currentStamina;

        if (currentHealth <= 0)
        {
            healthBar.value = 0;
            Debug.Log("Player is dead");
        }

        if (isDrainingStamina)
        {
            regenCooldownTimer = staminaRegenDelay; // Reset cooldown when stamina is drained
        }
        else if (regenCooldownTimer > 0)
        {
            regenCooldownTimer -= Time.deltaTime; // Count down regen delay
        }
        else if (currentStamina < maxStamina)
        {
            RecoverStamina(); // Regenerate stamina if delay has passed
        }


    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    
    public void AddHealth(int value)
    {
        if (currentHealth + value >= maxHealth) currentHealth = maxHealth;
        currentHealth += value;
    }

    private void RecoverStamina()
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }

    public void UseStamina(float value)
    {
        if (currentStamina > 0)
        {
            currentStamina -= value; // Drain stamina
            currentStamina = Mathf.Max(currentStamina, 0); // Clamp to 0
            isDrainingStamina = true;
        }
    }

    public void StopUsingStamina()
    {
        isDrainingStamina = false; // Stop draining stamina
    }
}
