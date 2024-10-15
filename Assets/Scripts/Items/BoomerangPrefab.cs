using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangPrefab : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;
    private Transform playerTransform;
    private bool isReturning = false;
    private float currentRotation = 0f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        currentRotation += 30;

        if (currentRotation >= 360f) {
            currentRotation -= 360f;
        }

        transform.rotation = Quaternion.Euler(0, 0, currentRotation);

        if (isReturning) {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
        } else {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Chest")) {
            isReturning = true;
        }

        if (collision.CompareTag("Player") && isReturning) {
            isReturning = false;
            Destroy(gameObject);
        }
    }
}
