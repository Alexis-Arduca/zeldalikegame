using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerLife playerLife;
    private GameManager gameManager;
    private List<Key> playerKeys = new List<Key>();
    private bool canMove = true;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLife = GetComponent<PlayerLife>();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            inventory = gameManager.GetInventory();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }

        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
    }

    private void OnActionChange()
    {
        canMove = !canMove;
    }

    void Update()
    {
        if (canMove)
        {
            playerMovement.HandleMovement();

            if (Input.GetKeyDown(KeyCode.E))
            {
                Item equippedItemLeft = inventory.GetEquippedItem(true);
                if (equippedItemLeft != null)
                {
                    equippedItemLeft.Use();
                }
                else
                {
                    Debug.Log("No item equipped on Left Click.");
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Item equippedItemRight = inventory.GetEquippedItem(false);
                if (equippedItemRight != null)
                {
                    equippedItemRight.Use();
                }
                else
                {
                    Debug.Log("No item equipped on Right Click.");
                }
            }
        }

        ///====================///
            /// Debug test command ///
            ///====================///

            // RedPotion
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ObtainPotion(1);
            }
        // BluePotion
        if (Input.GetKeyDown(KeyCode.X))
        {
            ObtainPotion(2);
        }
        // Fairy
        if (Input.GetKeyDown(KeyCode.C))
        {
            ObtainPotion(3);
        }
        // Lose Heart
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerLife.TakeDamage(1);
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

    public void ObtainPotion(int potion)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];

            if (item is Bottle bottle && bottle.IsEmpty() == true)
            {
                if (potion == 1) {
                    bottle.SetRedPotion();
                } else if (potion == 2) {
                    bottle.SetBluePotion();
                } else if (potion == 3) {
                    bottle.SetFairy();
                }
            }
        }
    }

    public void ObtainNewKey(Key newKey)
    {
        playerKeys.Add(newKey);
    }
}
