using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectibles : MonoBehaviour
{
    public GameObject[] drops;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            int random = Random.Range(0, 10);

            if (random > 8) {
                Instantiate(drops[Random.Range(0, drops.Length)], gameObject.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
