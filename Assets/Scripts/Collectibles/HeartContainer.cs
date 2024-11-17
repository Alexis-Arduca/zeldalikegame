using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    public PlayerLife playerLife;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLife.HeartContainerUpdater();
            GameEventsManager.instance.miscEvents.HeartContainerCollected();
            Destroy(gameObject);
        }
    }
}
