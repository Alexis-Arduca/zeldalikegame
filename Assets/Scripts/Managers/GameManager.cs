using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite bowSprite;
    public Sprite longshotSprite;
    public GameObject arrowPrefab;
    public GameObject longshotPrefab;
    public static GameManager instance;
    public Inventory inventory;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inventory = new Inventory();

        // inventory.AddItem(new Bow(bowSprite, arrowPrefab));
        // inventory.AddItem(new Longshot(longshotSprite, longshotPrefab));
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
