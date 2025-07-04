using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void SavePlayer(PlayerController player)
    {
        PlayerData data = CreatePlayerData(player);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game saved at " + SavePath);
    }

    public static PlayerData LoadPlayer()
    {
        if (!SaveExists())
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("Game loaded from " + SavePath);
        return data;
    }

    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }

    public static void DeleteSave()
    {
        if (SaveExists())
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted.");
        }
    }

    private static PlayerData CreatePlayerData(PlayerController player)
    {
        PlayerData data = new PlayerData();

        PlayerLife life = player.GetComponent<PlayerLife>();
        data.currentHealth = life.GetCurrentHeart();
        data.maxHealth = life.GetMaxHeart();
        data.fragment = life.heartFragment;

        PlayerMagic magic = player.GetComponent<PlayerMagic>();
        data.currentMagic = magic.GetCurrentMagic();
        data.maxMagic = magic.GetMaxMagic();
        data.magicLevel = magic.GetCurrentLevel();

        data.hasShield = true;

        data.lastPosition = player.transform.position;

        data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        PlayerResources ressources = player.GetComponent<PlayerResources>();
        data.currentArrow = ressources.currentArrow;
        data.maxArrow = ressources.maxArrow;
        data.quiverLevel = ressources.quiverLevel;
        data.currentBomb = ressources.currentBomb;
        data.maxBomb = ressources.maxBomb;
        data.bombBagLevel = ressources.bombBagLevel;

        Inventory inventory = player.GetInventory();
        foreach (Item item in inventory.items)
        {
            ItemData itemData = new ItemData
            {
                itemId = item.itemName
            };
            data.inventoryItems.Add(itemData);
        }

        return data;
    }
}
