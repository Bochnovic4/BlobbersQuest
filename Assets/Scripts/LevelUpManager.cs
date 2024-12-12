using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    private Player player;
    private PlayerController playerController;

    [Header("UI")]
    [SerializeField]
    private GameObject lvlUpPanel;
    [SerializeField]
    private GameObject interactPanel;
    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private TMP_Text expText;
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private TMP_Text staminaText;
    [SerializeField]
    private TMP_Text strenghtText;
    [SerializeField]
    private TMP_Text defenceText;

    [SerializeField]
    private TMP_Text lvlUpCostText;

    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private PlayerData playerData;
    private float lvlUpCost;

    private bool isInRange = false;
    private bool isInteracting = false;
    private TMP_Text interactText;

    void Start()
    {
        lvlUpPanel.SetActive(false);
        interactPanel.SetActive(false);
        player = playerObject.GetComponent<Player>();
        playerController = playerObject.GetComponent<PlayerController>();
        interactText = interactPanel.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < 10f)
        {
            interactPanel.SetActive(true);
            isInRange = true;

            if (isInteracting)
            {
                interactText.text = "Press E to save and quit";
            }
            else
            {
                interactText.text = "Press E to interact";
            }
        }
        else
        {
            interactPanel.SetActive(false);
            isInRange = false;

            if (isInteracting)
            {
                Quit();
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isInRange)
            {
                if (isInteracting)
                {
                    Quit();
                }
                else
                {
                    Interact();
                }
            }
        }
    }

    public void Interact()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        isInteracting = true;
        playerController.isCameraLocked = true;
        playerController.enabled = false;

        lvlUpCost = (int)(100 * ((playerData.lvl / 10) + 1));
        lvlUpPanel.SetActive(true);
        UpdateUI();
        RestorePlayerStats();

        buttons.SetActive(playerData.exp >= lvlUpCost);
    }

    public void Quit()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        isInteracting = false;
        playerController.isCameraLocked = false;
        playerController.enabled = true;

        dataManager.savePlayerData();
        player.ResetStats();
        lvlUpPanel.SetActive(false);
    }

    public void BuyHealth()
    {
        UpgradeStat(() => player.AddHealth(20f));
    }

    public void BuyStamina()
    {
        UpgradeStat(() => player.AddStamina(20f));
    }

    public void BuyStrenght()
    {
        UpgradeStat(() => player.AddStrenght(1f));
    }

    public void BuyDefence()
    {
        UpgradeStat(() => player.AddDefence(1f));
    }

    private void UpgradeStat(System.Action statUpgradeAction)
    {
        playerData.exp -= lvlUpCost;
        playerData.lvl += 1;
        statUpgradeAction.Invoke();
        UpdateUI();
    }

    private void UpdateUI()
    {
        lvlUpCost = (int)(100 * ((playerData.lvl / 10) + 1));
        expText.text = "Exp: " + playerData.exp;
        defenceText.text = "Defence: " + playerData.defence;
        strenghtText.text = "Strength: " + playerData.strenght;
        staminaText.text = "Stamina: " + playerData.stamina;
        healthText.text = "Health: " + playerData.health;
        lvlUpCostText.text = "Cost: " + lvlUpCost;

        buttons.SetActive(playerData.exp >= lvlUpCost);
    }

    private void RestorePlayerStats()
    {
        player.currentHealth = player.maxHealth;
        player.currentStamina = player.maxStamina;
    }
}
