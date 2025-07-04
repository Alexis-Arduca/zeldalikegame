using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Health
    public double currentHealth;
    public int maxHealth;
    public int fragment;
    
    // Magic
    public int currentMagic;
    public int maxMagic;
    public int magicLevel;

    // Inventory
    public List<ItemData> inventoryItems = new();
    public List<KeyData> keys = new();

    // Shield
    public bool hasShield;
    public bool isShieldActive;
    public Vector2 shieldDirection;

    // Ressources
    public int currentArrow;
    public int maxArrow;
    public int quiverLevel;
    public int currentBomb;
    public int maxBomb;
    public int bombBagLevel;

    // Unity Parameter
    public Vector2 lastPosition;
    public string currentScene;
}

[System.Serializable]
public class ItemData
{
    public string itemId;
}

[System.Serializable]
public class KeyData
{
    public string keyId;
}
