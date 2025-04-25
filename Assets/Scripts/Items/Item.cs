using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public bool isKey;
    public bool isBossKey;

    public string itemName;
    public bool isEquipped;
    public int slotIndex;
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

    public virtual bool IsKey()
    {
        return isKey;
    }

    public virtual bool IsBossKey()
    {
        return isBossKey;
    }
}
