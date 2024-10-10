using UnityEngine;
using UnityEngine.UI; // Assurez-vous d'utiliser UnityEngine.UI pour Image
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Inventory inventory;
    public Image[] itemImages;
    private int selectedIndex;

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
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleInventory();
        }

        if (inventoryPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < itemImages.Length; i++)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(itemImages[i].rectTransform, Input.mousePosition))
                    {
                        selectedIndex = i;
                        EquipSelectedItem();
                        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                        break;
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
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                Item item = inventory.items[i];
                itemImages[i].sprite = item.itemSprite;
                itemImages[i].color = item.isEquipped ? Color.yellow : Color.white;
                itemImages[i].gameObject.SetActive(true);
            }
            else
            {
                itemImages[i].gameObject.SetActive(false);
            }
        }
    }

    void EquipSelectedItem()
    {
        if (selectedIndex < inventory.items.Count)
        {
            inventory.UnequipItem(inventory.GetEquippedItem()?.itemName);
            inventory.EquipItem(inventory.items[selectedIndex].itemName);
            UpdateItemDisplay();
        }
    }
}
