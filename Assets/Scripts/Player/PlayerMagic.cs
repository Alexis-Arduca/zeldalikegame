using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private int currentMagic;
    private int maxMagic;
    private int magicLevel;
    
    void Start()
    {
        currentMagic = 20;
        maxMagic = 100;
        magicLevel = 1;

        GameEventsManager.instance.magicEvents.onMagicCollected += RefillMagic;
        GameEventsManager.instance.magicEvents.onMagicUsed += ConsumeMagic;
    }

    void OnDisable()
    {
        GameEventsManager.instance.magicEvents.onMagicCollected -= RefillMagic;
        GameEventsManager.instance.magicEvents.onMagicUsed += ConsumeMagic;
    }

    public void RefillMagic(int value)
    {
        currentMagic += value;

        if (currentMagic > maxMagic) {
            currentMagic = maxMagic;
        }
    }
    
    public bool CanUse(int cost)
    {
        return cost < currentMagic ? true : false;
    }

    public void ConsumeMagic(int cost)
    {
        currentMagic -= cost;
    }

    public int GetCurrentMagic()
    {
        return currentMagic;
    }

    public int GetCurrentLevel()
    {
        return magicLevel;
    }

    public void UpgradeMagicLevel()
    {
        magicLevel += 1;

        maxMagic = 100 + (magicLevel - 1) * 50;
    }
}
