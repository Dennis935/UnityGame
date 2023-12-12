using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenChooseLevel : MonoBehaviour
{
    public void OpenLevel()
    {
        SceneManager.LoadScene("LevelChoosing");

    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level1");

    }

    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level2");

    }
    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level3");

    }
    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level4");

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("GameStart");

    }
}
