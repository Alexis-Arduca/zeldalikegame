using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicUI : MonoBehaviour
{
    public PlayerMagic playerMagic;
    private Image magicBar;
    private RectTransform magicBarRect;

    void Start()
    {
        magicBar = gameObject.GetComponent<Image>();
        magicBarRect = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (playerMagic != null)
        {
            int currentMagic = playerMagic.GetCurrentMagic();
            int playerLevel = playerMagic.GetCurrentLevel();

            int maxMagic = GetMaxMagicForLevel(playerLevel);

            float fillAmount = Mathf.Clamp01((float)currentMagic / (float)maxMagic);
            magicBar.fillAmount = fillAmount;
        }
    }

    private int GetMaxMagicForLevel(int level)
    {
        switch (level)
        {
            case 1: return 100;
            case 2: return 150;
            case 3: return 200;
            default: return 100;
        }
    }
}
