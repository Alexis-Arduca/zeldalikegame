using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Item> items;
    public List<Item> availableItems;

    public Inventory()
    {
        items = new List<Item>();
        availableItems = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item != null && !items.Contains(item))
        {
            items.Add(item);
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName && i.isEquipped);
    }

    public void EquipItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                item.isEquipped = true;
                Debug.Log($"{itemName} has been equipped.");
                break;
            }
        }
    }

    public void UnequipItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                item.isEquipped = false;
                Debug.Log($"{itemName} has been unequipped.");
                break;
            }
        }
    }

    public void PrintInventory()
    {
        foreach (Item item in items)
        {
            string equippedStatus = item.isEquipped ? " (Equipped)" : "";
            Debug.Log(item.itemName + equippedStatus);
        }
    }

    public Item GetEquippedItem()
    {
        return items.Find(item => item.isEquipped);
    }
}
