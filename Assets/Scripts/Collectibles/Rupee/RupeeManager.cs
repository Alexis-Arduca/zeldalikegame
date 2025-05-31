using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RupeeManager : MonoBehaviour
{
    private int rupeeCount;
    private int maxRupee;
    private int bagLevel;

    void Start()
    {
        rupeeCount = 0;
        maxRupee = 99;
        bagLevel = 1;
        GameEventsManager.instance.rupeeEvents.onRupeeCollected += RupeeCollected; 
        GameEventsManager.instance.rupeeEvents.onRupeeUsed += RupeeUsed;
    }

    void OnDisable()
    {
        GameEventsManager.instance.rupeeEvents.onRupeeCollected -= RupeeCollected;
        GameEventsManager.instance.rupeeEvents.onRupeeUsed -= RupeeUsed; 
    }

    private void RupeeCollected(int rupee)
    {
        if (rupeeCount < maxRupee) {
            if (rupeeCount + rupee > maxRupee) {
                rupeeCount = maxRupee;
            } else {
                rupeeCount += rupee;
            }
        }
    }

    private void RupeeUsed(int rupee)
    {
        rupeeCount -= rupee;

        if (rupeeCount < 0) { rupeeCount = 0; }
    }

    private void UpgradeBag()
    {
        if (bagLevel < 4)
        {
            bagLevel += 1;
        }

        if (bagLevel == 2)
        {
            maxRupee = 200;
        }
        else if (bagLevel == 3)
        {
            maxRupee = 500;
        }
        else if (bagLevel == 4)
        {
            maxRupee = 999;
        }
    }

    public int GetRupee()
    {
        return rupeeCount;
    }
}
