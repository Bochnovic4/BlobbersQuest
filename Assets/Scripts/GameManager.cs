using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes

        if (GameHandler.isNewGame)
            StartNewGame();
        else
            LoadGame();
        menuPanel.SetActive(isMenuPanelOn);
        Cursor.visible = false;
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
