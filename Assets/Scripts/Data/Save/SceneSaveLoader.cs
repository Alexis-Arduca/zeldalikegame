using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneSaveLoader : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;

    private string saveFileName = "save.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    void Start()
    {
        if (SceneLoadInfo.LoadedFromSave)
        {
            SceneLoadInfo.LoadedFromSave = false;
            SpawnPlayerFromSave();
        }
        else
        {
            SpawnPlayer();
        }

        GameEventsManager.instance.playerEvents.OnPlayerSpawn();
    }

    private void SpawnPlayerFromSave()
    {
        if (!File.Exists(SavePath))
        {
            SpawnPlayer();
            return;
        }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        GameObject playerObj = SpawnPlayer();

        PlayerController player = playerObj.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LoadFromData(data);
        }
        else
        {
            Debug.LogError("PlayerController not found on spawned player.");
        }
    }

    private GameObject SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned in SceneSaveLoader!");
            return null;
        }

        GameObject playerObj = Instantiate(playerPrefab, defaultSpawnPosition, Quaternion.identity);
        return playerObj;
    }
}
