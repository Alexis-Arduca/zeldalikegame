using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHeart : MonoBehaviour
{
    public PlayerLife playerLife;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerLife.HealFromCollectible(1);
            Destroy(gameObject);
        }
    }
}
