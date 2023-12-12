using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    private void Start()
    {
        LoadPlayerName();
    }

    public void SavePlayerName()
    {
        string playerName = playerNameInput.text;

        // �berpr�fe, ob ein g�ltiger Spielername eingegeben wurde
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
        }
    }


    private void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string playerName = PlayerPrefs.GetString("PlayerName");
            playerNameInput.text = playerName;
        }
    }

    public bool IsPlayerNameEntered()
    {
        return !string.IsNullOrEmpty(playerNameInput.text);
    }
}
