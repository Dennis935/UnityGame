using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameStart");
    }
}
