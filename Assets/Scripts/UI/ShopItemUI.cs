using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public Button buyButton;

    private GameObject item;
    private int price;
    private ShopManager shopManager;

    public void Setup(GameObject item, int price, ShopManager manager)
    {
        this.item = item;
        this.price = price;
        shopManager = manager;

        itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        itemNameText.text = item.name;

        itemPriceText.text = price.ToString() + "Rupees";

        buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnBuyClicked()
    {
        shopManager.BuyItem(item, price);
    }
}
