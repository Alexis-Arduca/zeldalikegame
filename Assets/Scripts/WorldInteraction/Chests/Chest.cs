using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openChest;
    public Sprite closeChest;
    private bool isOpen;
    private SpriteRenderer spriteRenderer;

    protected GameObject collectibleInChest;
    protected Item itemInChest;

    void Start()
    {
        isOpen = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeChest;
    }

    public virtual void FillChest(GameObject collectiblePrefab)
    {
        collectibleInChest = collectiblePrefab;
    }

    public virtual void FillChest(Item item)
    {
        itemInChest = item;
    }

    protected virtual void OpenChest(PlayerController player)
    {
        isOpen = true;
        spriteRenderer.sprite = openChest;

        if (itemInChest != null)
        {
            player.ObtainItem(itemInChest);
            SpawnFloating(itemInChest.itemSprite);
            itemInChest = null;
        }
        else if (collectibleInChest != null)
        {
            var spawned = Instantiate(
                collectibleInChest,
                player.transform.position,
                Quaternion.identity
            );
            SpawnFloating(spawned.GetComponent<SpriteRenderer>().sprite);
            collectibleInChest = null;
        }
    }

    private void SpawnFloating(Sprite sprite)
    {
        var display = new GameObject("ItemDisplay");
        display.transform.position = transform.position + Vector3.up * 0.5f;
        var sr = display.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        display.AddComponent<ChestAnimation>().StartFloating();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.O) && !isOpen)
        {
            OpenChest(other.GetComponent<PlayerController>());
        }
    }
}
