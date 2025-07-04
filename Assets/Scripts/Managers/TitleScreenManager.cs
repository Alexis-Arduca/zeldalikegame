using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/save.json";
    public GameObject loadSaveButton;
    public GameObject deleteSaveButton;

    void Start()
    {
        if (!SaveExists())
        {
            loadSaveButton.GetComponent<Button>().interactable = false;
            deleteSaveButton.GetComponent<Button>().interactable = false;
        }
    }

    private static bool SaveExists()
    {
        return File.Exists(SavePath);
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSaveLoad()
    {
        if (!SaveExists()) { return; }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        SceneLoadInfo.LoadedFromSave = true;
        SceneManager.LoadScene(data.currentScene);
    }

    public void OnSaveDelete()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            loadSaveButton.GetComponent<Button>().interactable = false;
            deleteSaveButton.GetComponent<Button>().interactable = false;
        }
    }
}
