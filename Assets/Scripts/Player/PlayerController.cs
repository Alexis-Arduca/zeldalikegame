using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Inventory inventory;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            inventory = gameManager.inventory;
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
        }
    }


    public Inventory GetInventory()
    {
        return inventory;
    }

    // Méthode pour simuler l'obtention d'un nouvel objet
    public void ObtainItem(Item item)
    {
        inventory.AddItem(item);
        Debug.Log($"Item obtenu: {item.itemName}");
    }

}
