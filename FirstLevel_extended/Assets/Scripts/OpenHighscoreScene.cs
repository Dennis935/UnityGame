using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenHighscoreScene : MonoBehaviour
{
    public void OpenHighscore()
    {
        SceneManager.LoadScene("HighscoreScene");
    }
}
