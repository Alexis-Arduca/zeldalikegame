using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Shop : NPC
{
    public GameObject shopPanel;
    public List<GameObject> itemsForSale;
    public List<int> itemsPrice;
    private bool hasTraded = false;

    protected override void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O))
        {
            if (!hasTraded)
            {
                DialogueManager.Instance.StartDialogue(dialogueData, 0);
                shopPanel.GetComponent<ShopManager>().OpenShop(this);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueData, 1);
                hasTraded = false;
            }
        }
    }

    public void OnTradeCompleted()
    {
        hasTraded = true;
        shopPanel.SetActive(false);
    }
}
