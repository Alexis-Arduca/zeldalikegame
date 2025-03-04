using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestKey : MonoBehaviour
{
    public Sprite openChest;
    public Sprite closeChest;
    public Key keyInChest;
    private bool isOpen;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        isOpen = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeChest;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.O))
        {
            if (!isOpen)
            {
                OpenChest(other.GetComponent<PlayerController>());
            }
        }
    }

    void OpenChest(PlayerController player)
    {
        isOpen = true;
        spriteRenderer.sprite = openChest;

        player.ObtainNewKey(keyInChest);

        GameObject itemDisplay = new GameObject("ItemDisplay");
        itemDisplay.transform.position = transform.position + new Vector3(0, 0.5f, 0);

        SpriteRenderer itemSpriteRenderer = itemDisplay.AddComponent<SpriteRenderer>(); 
        itemSpriteRenderer.sprite = keyInChest.GetComponent<SpriteRenderer>().sprite;

        itemDisplay.AddComponent<ChestAnimation>().StartFloating();
    }
}
