using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rupee : MonoBehaviour
{
    public int Value;
    public string RupeeName;

    public void Initialize(int value, string rupeeName)
    {
        Value = value;
        RupeeName = rupeeName;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.rupeeEvents.OnRupeeCollected(Value);
            Destroy(gameObject);
        }
    }
}
