using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Shop : NPC
{
    public GameObject shopPanel;
    public List<GameObject> itemsForSale;
    public List<int> itemsPrice;
    private bool isShopOpen = false;

    protected override void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O))
        {
            if (!isShopOpen)
            {
                GameEventsManager.instance.playerEvents.OnActionChange();
                DialogueManager.Instance.StartDialogue(dialogueData, 0);
                shopPanel.GetComponent<ShopManager>().OpenShop(this);
                isShopOpen = true;
            }
        }
    }

    public void OnTradeCompleted()
    {
        DialogueManager.Instance.StartDialogue(dialogueData, 1);
    }

    public void OnShopClosed()
    {
        DialogueManager.Instance.StartDialogue(dialogueData, 2);
        isShopOpen = false;
    }
}
