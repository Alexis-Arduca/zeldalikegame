using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour
{
    public GameObject saveText;
    private bool isGameSaved = false;

    public void SaveGame()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null && !isGameSaved)
        {
            isGameSaved = true;
            SaveSystem.SavePlayer(player);
            StartCoroutine(AfterSaveCoroutine());
        }
    }

    private IEnumerator AfterSaveCoroutine()
    {
        saveText.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveText.SetActive(false);
        isGameSaved = false;
    }
}
