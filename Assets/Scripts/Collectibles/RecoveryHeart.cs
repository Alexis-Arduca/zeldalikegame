using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHeart : Collectibles
{
    public double nbHeart;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.collectibleEvents.OnRecoveryHeartCollected(nbHeart);
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.collectibleEvents.OnRecoveryHeartCollected(nbHeart);
    }
}
