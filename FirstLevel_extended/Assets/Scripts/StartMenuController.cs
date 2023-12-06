using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Lade die Level-1-Szene (ersetze "Level1" durch den Namen deiner Level-1-Szene)
        SceneManager.LoadScene("Level1");
    }
}
