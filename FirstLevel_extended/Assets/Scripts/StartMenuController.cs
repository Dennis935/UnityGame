using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public PlayerNameInput playerNameInput;

    public void StartGame()
    {
        if (playerNameInput.IsPlayerNameEntered())
        {
            playerNameInput.SavePlayerName();  // Hier wird die SavePlayerName-Methode aufgerufen
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("Bitte gib einen Spielername ein, bevor du das Spiel startest.");
        }
    }
}
