using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollectible : MonoBehaviour
{
    public PlayerResources playerResources;
    public int nbArrow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerResources.RefillArrow(nbArrow);
            Destroy(gameObject);
        }
    }
}
