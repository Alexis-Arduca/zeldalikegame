using UnityEngine;
using UnityEngine.UI;
using System;

public class LifeUI : MonoBehaviour
{
    public PlayerLife playerLife;
    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        double currentHearts = playerLife.GetCurrentHeart();
        int maxHearts = (int)System.Math.Ceiling(playerLife.GetMaxHeart());

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < maxHearts);

            if (i < maxHearts)
            {
                if (currentHearts >= i + 1)
                {
                    heartImages[i].sprite = fullHeartSprite;
                }
                else if (currentHearts >= i + 0.5f)
                {
                    heartImages[i].sprite = halfHeartSprite;
                }
                else
                {
                    heartImages[i].sprite = emptyHeartSprite;
                }
            }
        }
    }
}
