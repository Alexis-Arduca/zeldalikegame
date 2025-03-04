using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Dungeon dungeon;
    public bool isBossKey;
    public bool isOnFloor = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOnFloor == true)
        {
            other.gameObject.GetComponent<PlayerController>().ObtainNewKey(this);
            Destroy(gameObject);
        }
    }
}
