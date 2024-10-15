using UnityEngine;
using UnityEngine.UI; // Assurez-vous d'utiliser UnityEngine.UI pour Image
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Inventory inventory;
    public Image[] itemImages;
    private int selectedIndex;

    public Sprite redPotionSprite;
    public Sprite bluePotionSprite;
    public Sprite fairySprite;
    public Sprite emptySprite;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            ToggleInventory();
        }

        if (inventoryPanel.activeSelf) {
            if (Input.GetMouseButtonDown(0)) {
                foreach (Item item in inventory.items) {
                    int slotIndex = item.slotIndex;

                    if (slotIndex >= 0 && slotIndex < itemImages.Length && itemImages[slotIndex].gameObject.activeSelf) {
                        if (RectTransformUtility.RectangleContainsScreenPoint(itemImages[slotIndex].rectTransform, Input.mousePosition)) {
                            selectedIndex = slotIndex;
                            EquipSelectedItem();
                            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                            break;
                        }
                    }
                }
            }
        }
    }


    void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            selectedIndex = 0;
            UpdateItemDisplay();
        }
    }

    void UpdateItemDisplay()
    {
        for (int i = 0; i < itemImages.Length; i++) {
            itemImages[i].gameObject.SetActive(false);
        }

        foreach (Item item in inventory.items) {
            int slotIndex = item.slotIndex;

            if (slotIndex >= 0 && slotIndex < itemImages.Length) {
                if (item is Bottle bottle) {
                    if (bottle.GetRedPotion()) {
                        itemImages[slotIndex].sprite = redPotionSprite;
                    } else if (bottle.GetBluePotion()) {
                        itemImages[slotIndex].sprite = bluePotionSprite;
                    } else if (bottle.GetFairy()) {
                        itemImages[slotIndex].sprite = fairySprite;
                    } else {
                        itemImages[slotIndex].sprite = emptySprite;
                    }

                    itemImages[slotIndex].color = item.isEquipped ? Color.yellow : Color.white;
                }
                else
                {
                    itemImages[slotIndex].sprite = item.itemSprite;
                    itemImages[slotIndex].color = item.isEquipped ? Color.yellow : Color.white;
                }

                itemImages[slotIndex].gameObject.SetActive(true);
            }
        }
    }


    void EquipSelectedItem()
    {
        inventory.UnequipItem(inventory.GetEquippedItem()?.itemName);

        Item itemToEquip = null;

        foreach (Item item in inventory.items) {
            if (item.slotIndex == selectedIndex) {
                itemToEquip = item;
                break;
            }
        }

        if (itemToEquip != null) {
            inventory.EquipItem(itemToEquip.itemName);
        }

        UpdateItemDisplay();
    }
}
