using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
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
