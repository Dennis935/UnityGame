using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighscoreDisplay : MonoBehaviour
{
    public TMP_Text[] playerNameTexts; 
    public TMP_Text[] scoreTexts; 

    private const int MaxHighscores = 5; 

    private List<HighscoreEntry> highscores;

    [System.Serializable]
    private class HighscoreEntry
    {
        public string playerName;
        public int score;
    }

    private void Start()
    {
        LoadHighscores();
        DisplayHighscores();
        UpdateHighscores(PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("Score"));
    }

    private void LoadHighscores()
    {
        highscores = new List<HighscoreEntry>();

        for (int i = 0; i < MaxHighscores; i++)
        {
            string playerNameKey = "PlayerName" + i;
            string scoreKey = "Score" + i;

            if (PlayerPrefs.HasKey(playerNameKey) && PlayerPrefs.HasKey(scoreKey))
            {
                HighscoreEntry entry = new HighscoreEntry
                {
                    playerName = PlayerPrefs.GetString(playerNameKey),
                    score = PlayerPrefs.GetInt(scoreKey)
                };

                highscores.Add(entry);
            }
            else
            {
              
                break;
            }
        }

        highscores.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private void SaveHighscores()
    {
        for (int i = 0; i < Mathf.Min(highscores.Count, MaxHighscores); i++)
        {
            string playerNameKey = "PlayerName" + i;
            string scoreKey = "Score" + i;

            PlayerPrefs.SetString(playerNameKey, highscores[i].playerName);
            PlayerPrefs.SetInt(scoreKey, highscores[i].score);
        }

        PlayerPrefs.Save();
    }

    public void UpdateHighscores(string playerName, int score)
    {
        int existingIndex = -1;
        for (int i = 0; i < highscores.Count; i++)
        {
            if (highscores[i].playerName == playerName)
            {
                existingIndex = i;
                break;
            }
        }

        if (existingIndex != -1 && score > highscores[existingIndex].score)
        {
            highscores[existingIndex].score = score;
            highscores.Sort((a, b) => b.score.CompareTo(a.score));
        }
        else if (existingIndex == -1 && highscores.Count < MaxHighscores)
        {
            HighscoreEntry newEntry = new HighscoreEntry
            {
                playerName = playerName,
                score = score
            };

            highscores.Add(newEntry);
            highscores.Sort((a, b) => b.score.CompareTo(a.score)); 
        }

        SaveHighscores();

        DisplayHighscores();
    }

    private void DisplayHighscores()
    {
        for (int i = 0; i < Mathf.Min(highscores.Count, MaxHighscores); i++)
        {
            playerNameTexts[i].text = "Player: " + highscores[i].playerName;
            scoreTexts[i].text = "Score: " + highscores[i].score;
        }
    }
}
