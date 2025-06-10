using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerLife playerLife;
    private Inventory inventory;
    private List<Key> playerKeys = new List<Key>();
    private bool canMove = true;

    void Start()
    {
        // Initialize components
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLife = GetComponent<PlayerLife>();
        inventory = FindObjectOfType<GameManager>()?.GetInventory();

        if (inventory == null)
        {
            Debug.LogError("Inventory not found!");
        }

        // Subscribe to input events
        playerInput.OnMoveInput += (direction) => { if (canMove) playerMovement.HandleMovement(direction); }; // Passer la direction
        playerInput.OnAttackInput += () => { if (canMove) playerAttack.PerformAttack(playerInput.AttackDirection); };
        playerInput.OnUseLeftItemInput += UseLeftItem;
        playerInput.OnUseRightItemInput += UseRightItem;
        playerInput.OnDebugPotionInput += ObtainPotion;
        playerInput.OnDebugTakeDamageInput += () => playerLife.TakeDamage(1);

        // Subscribe to game events
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

    private void UseLeftItem()
    {
        Item equippedItem = inventory.GetEquippedItem(true);
        if (equippedItem != null)
        {
            equippedItem.Use();
        }
        else
        {
            Debug.Log("No item equipped on Left Click.");
        }
    }

    private void UseRightItem()
    {
        Item equippedItem = inventory.GetEquippedItem(false);
        if (equippedItem != null)
        {
            equippedItem.Use();
        }
        else
        {
            Debug.Log("No item equipped on Right Click.");
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
        foreach (Item item in inventory.items)
        {
            if (item is Bottle bottle && bottle.IsEmpty())
            {
                if (potion == 1) bottle.SetRedPotion();
                else if (potion == 2) bottle.SetBluePotion();
                else if (potion == 3) bottle.SetFairy();
                break;
            }
        }
    }

    public void ObtainNewKey(Key newKey)
    {
        playerKeys.Add(newKey);
    }
}
