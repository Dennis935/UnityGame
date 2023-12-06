using UnityEngine;
using TMPro;

public class HighscoreDisplay : MonoBehaviour
{
    public TMP_Text highscoreText;

    private void Start()
    {
        // Lade den Highscore und den Spielername
        string playerName = PlayerPrefs.GetString("PlayerName", "DefaultName");
        int highscore = PlayerPrefs.GetInt(playerName + "_Highscore", 0);

        // Zeige den Highscore an
        highscoreText.text = $"Highscore for {playerName}: {highscore}";
    }
}
