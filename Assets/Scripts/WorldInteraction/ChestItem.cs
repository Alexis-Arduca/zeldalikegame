using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    public Sprite openChest;
    public Sprite closeChest;
    public Item itemInChest;
    private bool isOpen;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        isOpen = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeChest;
    }

    void Update()
    {}

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Enter");

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.O))
        {
            if (!isOpen)
            {
                OpenChest(other.GetComponent<PlayerController>());
            }
        }
    }

    private void OnExitStay2D(Collider2D other)
    {
        Debug.Log("Exit");
    }

    void OpenChest(PlayerController player)
    {
        isOpen = true;
        spriteRenderer.sprite = openChest;

        if (itemInChest != null)
        {
            player.ObtainItem(itemInChest);
            Debug.Log($"{itemInChest.itemName} obtenu !");
        }
    }
}
