using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsPanel;
    private bool isCreditsOpen = false;

    public void StartGame(bool isNewGame)
    {
        GameHandler.isNewGame = isNewGame;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleCreditsPanel()
    {
        isCreditsOpen = !isCreditsOpen;
        creditsPanel.SetActive(isCreditsOpen);
    }

}
