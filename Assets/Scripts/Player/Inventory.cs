using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Item> items;
    public List<Item> availableItems;
    private Item equippedItemLeftClick;
    private Item equippedItemRightClick;

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
        return items.Exists(i => i.itemName == itemName && (i == equippedItemLeftClick || i == equippedItemRightClick));
    }

    public void EquipItem(string itemName, bool isLeftClick)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                if (isLeftClick)
                {
                    if (equippedItemLeftClick != null) equippedItemLeftClick.isEquipped = false;
                    equippedItemLeftClick = item;
                    Debug.Log($"{itemName} has been equipped on Left Click.");
                }
                else
                {
                    if (equippedItemRightClick != null) equippedItemRightClick.isEquipped = false;
                    equippedItemRightClick = item;
                    Debug.Log($"{itemName} has been equipped on Right Click.");
                }

                item.isEquipped = true;
                break;
            }
        }
    }

    public void UnequipItem(bool isLeftClick)
    {
        if (isLeftClick && equippedItemLeftClick != null)
        {
            Debug.Log($"{equippedItemLeftClick.itemName} has been unequipped from Left Click.");
            equippedItemLeftClick.isEquipped = false;
            equippedItemLeftClick = null;
        }
        else if (!isLeftClick && equippedItemRightClick != null)
        {
            Debug.Log($"{equippedItemRightClick.itemName} has been unequipped from Right Click.");
            equippedItemRightClick.isEquipped = false;
            equippedItemRightClick = null;
        }
    }

    public void PrintInventory()
    {
        foreach (Item item in items)
        {
            string equippedStatus = "";
            if (item == equippedItemLeftClick) equippedStatus = " (Equipped on Left Click)";
            else if (item == equippedItemRightClick) equippedStatus = " (Equipped on Right Click)";
            Debug.Log(item.itemName + equippedStatus);
        }
    }

    public Item GetEquippedItem(bool isLeftClick)
    {
        return isLeftClick ? equippedItemLeftClick : equippedItemRightClick;
    }
}
