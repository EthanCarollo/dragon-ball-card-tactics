using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // This function will be linked to the button
    public void QuitGame()
    {
        // Logs for testing in the editor
        Debug.Log("Quit Game button pressed!");

        // Closes the application (only works in a built game, not the editor)
        Application.Quit();
    }
}
