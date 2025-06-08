using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cucoo : MonoBehaviour
{
    [Header("Cucoo")]
    public float moveSpeed = 1.5f;
    public float moveTime = 2f;
    public float waitTime = 2f;
    public int maxHits = 3;
    public GameObject cucooReinforcementPrefab;
    public int numberOfReinforcements = 10;
    public float spawnRadius = 5f;
    public float reinforcementDelay = 0.5f;
    public float pickupRange = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float moveTimer;
    private float waitTimer;
    private int hitCount;
    private bool isAngry = false;
    private bool isPickedUp = false;
    private Transform playerTransform;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseNewDirection();
    }

    void Update()
    {
        if (isAngry || isPickedUp)
        {
            if (isPickedUp && Input.GetKeyDown(KeyCode.O))
            {
                Drop();
            }
    
            rb.velocity = Vector2.zero;
            if (isPickedUp && playerTransform != null)
            {
                transform.position = playerTransform.position + new Vector3(0, 0.5f, 0);
            }
            return;
        }

        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            rb.velocity = moveDirection * moveSpeed;

            if (moveTimer <= 0)
            {
                rb.velocity = Vector2.zero;
                waitTimer = waitTime;
            }
        }
        else if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                ChooseNewDirection();
            }
        }

        if (player != null && !isAngry)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= pickupRange && Input.GetKeyDown(KeyCode.O))
            {
                if (!isPickedUp)
                {
                    PickUp(player.transform);
                }
            }
        }
    }

    void ChooseNewDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        moveTimer = moveTime;
    }

    public void TakeHit()
    {
        if (isAngry || isPickedUp) return;

        hitCount++;

        if (hitCount >= maxHits)
        {
            StartCoroutine(CallReinforcements());
            isAngry = true;
        }
    }

    IEnumerator CallReinforcements()
    {
        for (int i = 0; i < numberOfReinforcements; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(cucooReinforcementPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(reinforcementDelay);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            TakeHit();
        }
        else if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    void PickUp(Transform player)
    {
        isPickedUp = true;
        playerTransform = player;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
    }

    void Drop()
    {
        isPickedUp = false;
        playerTransform = null;
        rb.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        ChooseNewDirection();
    }
}
