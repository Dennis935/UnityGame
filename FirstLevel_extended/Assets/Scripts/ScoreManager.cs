using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    public TMP_Text scoreText;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        // Finde das Textobjekt mit dem Namen "ScoreText" im Canvas
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

    }
}
