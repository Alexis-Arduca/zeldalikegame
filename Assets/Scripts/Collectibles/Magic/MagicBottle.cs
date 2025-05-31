using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBottle : Collectibles
{
    public int Value;

    public void Initialize(int value)
    {
        Value = value;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.magicEvents.OnMagicCollected(Value);
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.magicEvents.OnMagicCollected(Value);
    }
}
