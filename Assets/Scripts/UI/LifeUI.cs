using UnityEngine;
using UnityEngine.UI;
using System;

public class LifeUI : MonoBehaviour
{
    private bool playerLoad = false;
    private PlayerLife playerLife;
    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;

    void Start()
    {
        GameEventsManager.instance.playerEvents.onPlayerSpawn += InitPlayerLife;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerSpawn -= InitPlayerLife;
    }

    void Update()
    {
        if (playerLoad) { UpdateHearts(); }
    }

    private void InitPlayerLife()
    {
        playerLife = FindAnyObjectByType<PlayerLife>();
        playerLoad = true;
    }

    void UpdateHearts()
    {
        double currentHearts = playerLife.GetCurrentHeart();
        int maxHearts = playerLife.GetMaxHeart();

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
