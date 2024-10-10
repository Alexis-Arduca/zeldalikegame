using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Inventory inventory;
    public Sprite bowSprite;
    public Sprite longshotSprite;

    void Awake()
    {
        inventory = new Inventory();

        inventory.AddItem(new Bow(bowSprite));
        inventory.AddItem(new Longshot(longshotSprite));
    }
}
