using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenMainMenu : MonoBehaviour
{
    public void OpenMenu()
    {
        SceneManager.LoadScene("GameStart");
    }
}
