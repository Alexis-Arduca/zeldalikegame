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
    private bool hasShield = true;
    private bool isShieldActive = false;
    private Vector2 shieldDirection;
    private float shieldSpeedReduction = 0.5f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerLife = GetComponent<PlayerLife>();
        inventory = FindObjectOfType<GameManager>()?.GetInventory();

        if (inventory == null)
        {
            Debug.LogError("Inventory not found!");
        }

        playerInput.OnMoveInput += (direction) =>
        {
            if (canMove)
            {
                if (isShieldActive)
                {
                    playerMovement.HandleMovement(direction * shieldSpeedReduction);
                }
                else
                {
                    playerMovement.HandleMovement(direction);
                }
            }
        };
        playerInput.OnAttackInput += () => { if (canMove && !isShieldActive) playerAttack.PerformAttack(playerInput.AttackDirection); };
        playerInput.OnUseLeftItemInput += UseLeftItem;
        playerInput.OnUseRightItemInput += UseRightItem;
        playerInput.OnDebugPotionInput += ObtainPotion;

        playerInput.OnShieldInput += HandleShield;

        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
        playerInput.OnShieldInput -= HandleShield;
    }

    private void OnActionChange()
    {
        canMove = !canMove;
        if (!canMove && isShieldActive)
        {
            isShieldActive = false;
            UpdateShieldState();
        }
    }

    private void HandleShield(bool isPressed)
    {
        if (!hasShield) return;

        isShieldActive = isPressed;
        if (isShieldActive)
        {
            shieldDirection = playerInput.AttackDirection.normalized;
            if (shieldDirection == Vector2.zero)
            {
                shieldDirection = playerMovement.GetLastDirection();
            }
        }
        UpdateShieldState();
    }

    private void UpdateShieldState()
    {
        playerMovement.SetShieldState(isShieldActive, shieldDirection);
    }

    public bool ShieldState()
    {
        return isShieldActive;
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
        Debug.Log($"Item get: {item.itemName}");
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

    public void ObtainShield()
    {
        hasShield = true;
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    public Vector2 GetShieldDirection()
    {
        return shieldDirection;
    }
}
