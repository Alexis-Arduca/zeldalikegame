using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Inventory inventory;
    private GameManager gameManager;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            inventory = gameManager.GetInventory();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }

    void Update()
    {
        playerMovement.HandleMovement();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Item equippedItem = inventory.GetEquippedItem();
            if (equippedItem != null)
            {
                equippedItem.Use();
            }
            else
            {
                Debug.Log("No item equipped.");
            }
        }
    }


    public Inventory GetInventory()
    {
        return inventory;
    }

    public void ObtainItem(Item item)
    {
        inventory.AddItem(item);
        Debug.Log($"Item obtenu: {item.itemName}");
    }

}
