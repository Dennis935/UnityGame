using UnityEngine;
using UnityEngine.UI;

public class DeletePlayerPrefsButton : MonoBehaviour
{
    public void DeletePlayerPrefs()
    {
        // L�sche alle PlayerPrefs
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs wurden gel�scht.");
    }
}
