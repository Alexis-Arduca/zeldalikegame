using UnityEngine;

public class ChestItem : Chest
{
    public override void FillChest(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning($"Attempted to fill chest {name} with null item");
            return;
        }

        if (item.itemSprite == null)
        {
            Debug.LogWarning($"Item {item.name} for chest {name} has no sprite");
            return;
        }

        itemInChest = item;
        Debug.Log($"Chest {name} filled with item {item.name}");
    }
}