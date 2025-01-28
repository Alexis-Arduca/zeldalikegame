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
    }

    void OnDisable()
    {
        GameEventsManager.instance.rupeeEvents.onRupeeCollected -= RupeeCollected; 
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

    private void UpgradeBag()
    {
        if (bagLevel < 4) {
            bagLevel += 1;
        }

        if (bagLevel == 2) {
            maxRupee = 200;
        } else if (bagLevel == 3) {
            maxRupee = 500;
        } else if (bagLevel == 4) {
            maxRupee = 999;
        }
    }

    public int GetRupee()
    {
        return rupeeCount;
    }
}
