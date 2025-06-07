using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Inventory inventory;
    public QuestMenu questMenu;
    public Image[] itemImages;
    public Image equippedLeftImage;
    public Image equippedRightImage;
    private int selectedIndex;

    public Sprite redPotionSprite;
    public Sprite bluePotionSprite;
    public Sprite greenPotionSprite;
    public Sprite fairySprite;
    public Sprite emptySprite;

    private bool canOpenInv = true;

    void Start()
    {
        inventoryPanel.SetActive(false);
        selectedIndex = 0;

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            inventory = gameManager.GetInventory();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }

        UpdateItemDisplay();
        UpdateEquippedImages();

        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
    }

    private void OnActionChange()
    {
        canOpenInv = !canOpenInv;

        if (!canOpenInv) { inventoryPanel.SetActive(canOpenInv); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && canOpenInv) 
        {
            ToggleInventoryAndQuestMenu();
        }

        if (inventoryPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                bool isLeftClick = Input.GetMouseButtonDown(0);

                foreach (Item item in inventory.items)
                {
                    int slotIndex = item.slotIndex;

                    if (slotIndex >= 0 && slotIndex < itemImages.Length && itemImages[slotIndex].gameObject.activeSelf)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(itemImages[slotIndex].rectTransform, Input.mousePosition))
                        {
                            selectedIndex = slotIndex;
                            EquipSelectedItem(isLeftClick);
                            questMenu.questMenuPanel.SetActive(!questMenu.questMenuPanel.activeSelf);
                            ToggleInventoryAndQuestMenu();
                            break;
                        }
                    }
                }
            }
        }
    }

    void ToggleInventoryAndQuestMenu()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);
        
        if (isActive)
        {
            selectedIndex = 0;
            UpdateItemDisplay();
        }
    }

    void UpdateItemDisplay()
    {
        for (int i = 0; i < itemImages.Length; i++) 
        {
            itemImages[i].gameObject.SetActive(false);
        }

        foreach (Item item in inventory.items)
        {
            int slotIndex = item.slotIndex;

            if (slotIndex >= 0 && slotIndex < itemImages.Length) 
            {
                if (item is Bottle bottle)
                {
                    if (bottle.GetRedPotion())
                    {
                        itemImages[slotIndex].sprite = redPotionSprite;
                    } else if (bottle.GetBluePotion()) {
                        itemImages[slotIndex].sprite = bluePotionSprite;
                    } else if (bottle.GetGreenPotion()) {
                        itemImages[slotIndex].sprite = greenPotionSprite;
                    } else if (bottle.GetFairy()) {
                        itemImages[slotIndex].sprite = fairySprite;
                    } else {
                        itemImages[slotIndex].sprite = emptySprite;
                    }
                }
                else
                {
                    itemImages[slotIndex].sprite = item.itemSprite;
                }

                if (item == inventory.GetEquippedItem(true))
                {
                    itemImages[slotIndex].color = Color.yellow;
                }
                else if (item == inventory.GetEquippedItem(false))
                {
                    itemImages[slotIndex].color = Color.cyan;
                }
                else
                {
                    itemImages[slotIndex].color = Color.white;
                }

                itemImages[slotIndex].gameObject.SetActive(true);
            }
        }

        UpdateEquippedImages();
    }

    void EquipSelectedItem(bool isLeftClick)
    {
        inventory.UnequipItem(isLeftClick);

        Item itemToEquip = null;

        foreach (Item item in inventory.items)
        {
            if (item.slotIndex == selectedIndex)
            {
                itemToEquip = item;
                break;
            }
        }

        if (itemToEquip != null)
        {
            inventory.EquipItem(itemToEquip.itemName, isLeftClick);
        }

        UpdateItemDisplay();
    }

    void UpdateEquippedImages()
    {
        Item leftEquippedItem = inventory.GetEquippedItem(true);
        if (leftEquippedItem != null)
        {
            equippedLeftImage.sprite = leftEquippedItem.itemSprite;
            equippedLeftImage.color = Color.white;
        }
        else
        {
            equippedLeftImage.sprite = emptySprite;
            equippedLeftImage.color = new Color(1, 1, 1, 0);
        }

        Item rightEquippedItem = inventory.GetEquippedItem(false);
        if (rightEquippedItem != null)
        {
            equippedRightImage.sprite = rightEquippedItem.itemSprite;
            equippedRightImage.color = Color.white;
        }
        else
        {
            equippedRightImage.sprite = emptySprite;
            equippedRightImage.color = new Color(1, 1, 1, 0);
        }
    }
}
