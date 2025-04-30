using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FireRod rod;
    public IceRod rod2;
    public Ocarina ocarina;
    public Lamp lamp;

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

        inventory.AddItem(rod);
        inventory.AddItem(rod2);
        inventory.AddItem(ocarina);
        inventory.AddItem(lamp);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
