using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private GameObject menuPanel;
    private bool isMenuPanelOn = false;

    public bool IsMenuActive => isMenuPanelOn;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private GameObject gameFinishedPanel;

    [SerializeField]
    private Enemy bossEnemy;

    [SerializeField]
    private TMP_Text playerStatsText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (GameHandler.isNewGame)
            StartNewGame();
        else
            LoadGame();
        menuPanel.SetActive(isMenuPanelOn);
        gameFinishedPanel.SetActive(false);
        Cursor.visible = false;
    }

    public void OnBossDeath()
    {
        Time.timeScale = 0f;
        gameFinishedPanel.SetActive(true);

        playerStatsText.text = $"Level: {playerData.lvl}\nHealth: {playerData.health}\nStamina: " +
            $"{playerData.stamina}\nStrength: {playerData.strenght}\nDefence: {playerData.defence}";
    }

    public void RestartGame() {
        spawnManager.SpawnEnemies();
    }

    private void LoadGame()
    {
        dataManager.loadPlayerData();

    }

    private void StartNewGame()
    {
        dataManager.StartNewGame();
    }

    public void ReturnToMenu()
    {
        dataManager.savePlayerData();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        dataManager.savePlayerData();
        Application.Quit();
    }

    public void ToggleMenuPanel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMenuPanelOn = !isMenuPanelOn;
            menuPanel.SetActive(isMenuPanelOn);

            if (isMenuPanelOn)
            {
                
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            playerController.isCameraLocked = isMenuPanelOn;
            Cursor.visible = isMenuPanelOn;
        }

    }
}
