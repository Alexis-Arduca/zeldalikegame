using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Inventory inventory;
    private bool canMove = true;
    private bool hasShield = true;
    public bool isShieldActive = false;
    private Vector2 shieldDirection;
    private float shieldSpeedReduction = 0.5f;

    private List<Key> playerKeys = new List<Key>();

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        inventory = FindFirstObjectByType<GameManager>()?.GetInventory();

        if (inventory == null) Debug.LogError("Inventory not found!");

        playerInput.OnMoveInput += Move;
        playerInput.OnAttackInput += Attack;
        playerInput.OnUseLeftItemInput += UseLeftItem;
        playerInput.OnUseRightItemInput += UseRightItem;
        playerInput.OnShieldInput += HandleShield;

        GameEventsManager.instance.playerEvents.onActionState += ChangeState;
    }

    void OnDisable()
    {
        playerInput.OnMoveInput -= Move;
        playerInput.OnAttackInput -= Attack;
        playerInput.OnUseLeftItemInput -= UseLeftItem;
        playerInput.OnUseRightItemInput -= UseRightItem;
        playerInput.OnShieldInput -= HandleShield;

        GameEventsManager.instance.playerEvents.onActionState -= ChangeState;
    }

    public void LoadFromData(PlayerData data)
    {
        transform.position = data.lastPosition;

        PlayerLife life = GetComponent<PlayerLife>();
        life.LoadPlayerLife(data.maxHealth, data.currentHealth, data.fragment);

        PlayerMagic magic = GetComponent<PlayerMagic>();
        magic.LoadMagic(data.currentMagic, data.maxMagic, data.magicLevel);

        PlayerResources ressources = GetComponent<PlayerResources>();
        ressources.LoadRessources(data);

        Inventory myInv = Object.FindFirstObjectByType<GameManager>()?.GetComponent<Inventory>();
        myInv.LoadInventory(data.inventoryItems);

        hasShield = data.hasShield;
        isShieldActive = data.isShieldActive;
        shieldDirection = data.shieldDirection;
    }

    private void Move(Vector2 direction)
    {
        if (canMove)
        {
            playerMovement.HandleMovement(isShieldActive ? direction * shieldSpeedReduction : direction);
        }
    }

    private void Attack()
    {
        if (canMove && !isShieldActive) playerAttack.PerformAttack(Vector2.right);
    }

    private void UseLeftItem()
    {
        var item = inventory?.GetEquippedItem(true);
        if (item != null) item.Use();
        else Debug.Log("No item equipped on Left Click.");
    }

    private void UseRightItem()
    {
        var item = inventory?.GetEquippedItem(false);
        if (item != null) item.Use();
        else Debug.Log("No item equipped on Right Click.");
    }

    private void HandleShield(bool isPressed)
    {
        if (!hasShield) return;

        isShieldActive = isPressed;
        if (isShieldActive) shieldDirection = Vector2.right;
        UpdateShieldState();
    }

    private void UpdateShieldState()
    {
        playerMovement.SetShieldState(isShieldActive, shieldDirection);
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

    public void ChangeState()
    {
        canMove = !canMove;
    }
}
