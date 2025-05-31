using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Transform shopContent;
    public GameObject shopItemPrefab;

    public RupeeManager rupeeManager;
    private NPC_Shop currentNPC;

    public Transform playerTransform;

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OnCloseShop();
        }
    }

    public void OpenShop(NPC_Shop npc)
    {
        currentNPC = npc;

        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < npc.itemsForSale.Count; i++)
        {
            GameObject itemGO = Instantiate(shopItemPrefab, shopContent);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();
            itemUI.Setup(npc.itemsForSale[i], npc.itemsPrice[i], this);
        }

        gameObject.SetActive(true);
    }

    public void BuyItem(GameObject item, int price)
    {
        Debug.Log("Tentative d'achat de " + item.name + " pour " + price + " Rubis");

        if (rupeeManager.GetRupee() >= price)
        {
            GameEventsManager.instance.rupeeEvents.OnRupeeUsed(price);

            if (playerTransform != null)
            {
                item.GetComponent<Collectibles>().OnBuy();
            }
        }
        else
        {
            Debug.Log("Pas assez de Rubis");
        }
    }

    public void OnCloseShop()
    {
        gameObject.SetActive(false);

        if (currentNPC != null)
        {
            currentNPC.OnTradeCompleted();
            currentNPC = null;
        }
    }
}
