using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOfHeart : Collectibles
{
    public PlayerLife playerLife;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLife.HeartFragmentUpdater();
            GameEventsManager.instance.miscEvents.HeartFragmentCollected();
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.miscEvents.HeartFragmentCollected();
    }
}
