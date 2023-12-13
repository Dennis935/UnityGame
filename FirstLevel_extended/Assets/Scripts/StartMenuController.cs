using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    public PlayerNameInput playerNameInput;
    public TMP_Text notificationText;

    private void Start()
    {
        notificationText.enabled = false;
    }


    public void StartGame()
    {
        if (playerNameInput.IsPlayerNameEntered())
        {
            playerNameInput.SavePlayerName();
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("Add Gamertag to start the game");
            ShowNotification("Add Gamertag to start the game");
        }
    }

    private void ShowNotification(string message)
    {
        // Setze den Text und mache ihn sichtbar
        notificationText.text = message;
        notificationText.enabled = true;

        // Starte eine Coroutine, um die Benachrichtigung nach einigen Sekunden zu entfernen
        Invoke("HideNotification", 3f);
    }

    private void HideNotification()
    {
        // Verstecke die Benachrichtigung
        notificationText.enabled = false;
    }
}
