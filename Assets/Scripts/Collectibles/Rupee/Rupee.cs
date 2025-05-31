using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rupee : Collectibles
{
    public int Value;
    public string RupeeName;

    public void Initialize(int value, string rupeeName)
    {
        Value = value;
        RupeeName = rupeeName;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.rupeeEvents.OnRupeeCollected(Value);
            Destroy(gameObject);
        }
    }

    public override void OnBuy()
    {
        GameEventsManager.instance.rupeeEvents.OnRupeeCollected(Value);
    }
}
