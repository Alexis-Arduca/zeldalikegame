using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBottle : MonoBehaviour
{
    public int Value;

    public void Initialize(int value)
    {
        Value = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.magicEvents.OnMagicCollected(Value);
            Destroy(gameObject);
        }
    }
}
