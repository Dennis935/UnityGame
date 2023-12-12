using UnityEngine;
using UnityEngine.UI;

public class DeletePlayerPrefsButton : MonoBehaviour
{
    public void DeletePlayerPrefs()
    {
        // Lösche alle PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs wurden gelöscht.");
    }
}
