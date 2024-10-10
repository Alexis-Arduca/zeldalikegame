using UnityEngine;

public class Item
{
    public string itemName;
    public bool isEquipped;
    public Sprite itemSprite;

    public Item(string name, Sprite sprite)
    {
        itemName = name;
        isEquipped = false;
        itemSprite = sprite;
    }

    public virtual void Use()
    {
        Debug.Log($"{itemName} use !");
    }
}
