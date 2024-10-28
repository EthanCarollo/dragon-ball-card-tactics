using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    void StartGame()
    {
        // Assuming your game scene is named "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}