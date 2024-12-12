using System;
using System.Collections;
using Unity.Hierarchy;
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

    [SerializeField]
    public PlayerData playerData;

    public float strenght;
    public float defence;

    public float lvl;
    public float exp;
    [SerializeField]
    private DataManager dataManager;
    private RectTransform healthBarRect;
    private RectTransform staminaBarRect;
    [SerializeField]
    private GameObject deathPanel;
    [SerializeField]
    private Transform spawnTransform;
    void Start()
    {
        transform.position = spawnTransform.position;
        deathPanel.SetActive(false);
        healthBarRect = healthBar.GetComponent<RectTransform>();
        staminaBarRect = staminaBar.GetComponent<RectTransform>();
        maxHealth = playerData.health;
        maxStamina = playerData.stamina;
        strenght = playerData.strenght;
        defence = playerData.defence;
        lvl = playerData.lvl;
        exp = playerData.exp;

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;

        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthBar.value = currentHealth;
        staminaBar.value = currentStamina;

        if (currentHealth <= 0)
        {
            healthBar.value = 0;
            Die();
        }

        if (isDrainingStamina)
        {
            regenCooldownTimer = staminaRegenDelay;
        }
        else if (regenCooldownTimer > 0)
        {
            regenCooldownTimer -= Time.deltaTime;
        }
        else if (currentStamina < maxStamina)
        {
            RecoverStamina();
        }
    }

    private void Die()
    {
        exp = 0;
        transform.position = spawnTransform.position;
        ToggleDeathScreen();
    }

    private void ToggleDeathScreen()
    {
        deathPanel.SetActive(false);
        DelayAction(5);
        deathPanel.SetActive(true);
    }
    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    public void ResetStats()
    {
        maxStamina = playerData.stamina;
        maxHealth = playerData.health;
        strenght = playerData.strenght;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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
            currentStamina -= value;
            currentStamina = Mathf.Max(currentStamina, 0);
            isDrainingStamina = true;
        }
    }

    public void StopUsingStamina()
    {
        isDrainingStamina = false;
    }

    public void AddStamina(float value)
    {
        maxStamina += value;
        playerData.stamina += value;

        staminaBar.maxValue = maxStamina;
        staminaBarRect.sizeDelta = new Vector2(staminaBarRect.sizeDelta.x + maxStamina / 10f, staminaBarRect.sizeDelta.y);
    }

    public void AddHealth(float value)
    {
        maxHealth += value;
        playerData.health += value;

        healthBar.maxValue = maxHealth;
        healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x + maxHealth / 10f, healthBarRect.sizeDelta.y);
    }

    public void AddStrenght(float value)
    {
        strenght += value;
        playerData.strenght += value;
    }

    public void AddDefence(float value)
    {
        defence += value;
        playerData.defence += value;
    }
}
