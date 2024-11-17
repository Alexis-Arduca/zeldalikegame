using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFragmentSideQuest : QuestStep
{
    private int heartCollected = 0;
    private int heartToComplete = 4;

    private void Start()
    {
        GameEventsManager.instance.miscEvents.onHeartFragmentCollected += HeartCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onHeartFragmentCollected -= HeartCollected;
    }

    private void HeartCollected()
    {
        if (heartCollected < heartToComplete)
        {
            heartCollected++;
        }
        if (heartCollected >= heartToComplete)
        {
            FinishQuestStep();
        }
    }
}
