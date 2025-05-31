using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectible : Collectibles
{
    public int nbBomb;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.collectibleEvents.OnBombCollected(nbBomb);
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.collectibleEvents.OnBombCollected(nbBomb);
    }
}
