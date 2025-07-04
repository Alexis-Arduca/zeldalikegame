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
            Debug.LogWarning("Aucune sauvegarde trouvée à charger.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.LoadFromData(data);
            Debug.Log("Sauvegarde chargée depuis le fichier.");
        }
        else
        {
            Debug.LogError("Aucun PlayerController trouvé dans la scène.");
        }
    }
}
