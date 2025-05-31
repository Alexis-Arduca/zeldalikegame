using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : Collectibles
{
    public PlayerLife playerLife;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLife.HeartContainerUpdater();
            GameEventsManager.instance.miscEvents.HeartContainerCollected();
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.miscEvents.HeartContainerCollected();
    }
}
