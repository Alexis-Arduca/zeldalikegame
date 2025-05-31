using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollectible : Collectibles
{
    public int nbArrow;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.collectibleEvents.OnArrowCollected(nbArrow);
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.collectibleEvents.OnArrowCollected(nbArrow);
    }
}
