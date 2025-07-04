using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneSaveLoader : MonoBehaviour
{
    private string saveFileName = "save.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    void Start()
    {
        if (SceneLoadInfo.LoadedFromSave)
        {
            SceneLoadInfo.LoadedFromSave = false;
            LoadSave();
        }
    }

    private void LoadSave()
    {
        if (!File.Exists(SavePath))
        {
            return; }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.LoadFromData(data);
        }
    }
}
