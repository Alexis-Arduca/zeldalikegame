using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectible : MonoBehaviour
{
    public PlayerResources playerResources;
    public int nbBomb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerResources.RefillBomb(nbBomb);
            Destroy(gameObject);
        }
    }
}
