using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cucoo : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float moveTime = 2f;
    public float waitTime = 2f;
    public int maxHits = 3;
    public GameObject cucooReinforcementPrefab;
    public int numberOfReinforcements = 10;
    public float spawnRadius = 5f;
    public float reinforcementDelay = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float moveTimer;
    private float waitTimer;
    private int hitCount;
    private bool isAngry = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseNewDirection();
    }

    void Update()
    {
        if (isAngry)
        {
            rb.velocity = Vector2.zero;
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
    }

    void ChooseNewDirection()
    {
        moveDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        moveTimer = moveTime;
    }

    public void TakeHit()
    {
        if (isAngry) return;

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
    }
}
