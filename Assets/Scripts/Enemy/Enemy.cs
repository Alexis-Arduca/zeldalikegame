using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Patrolling, Chasing, Frozen }
    protected EnemyState currentState;

    public int health = 3;
    public int damage = 1;
    public float patrolSpeed = 2f;
    public float detectionRange = 5f;
    public float patrolPauseDuration = 2f;
    public float freezeDuration = 3f;

    protected Transform player;
    protected Vector2 patrolPoint;
    protected Vector2 lastDirection;
    protected float patrolPauseTimer;
    protected float freezeTimer;
    protected Rigidbody2D rb;

    protected Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    protected virtual void Start()
    {
        currentState = EnemyState.Idle;
        freezeTimer = 0;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        SetNewPatrolPoint();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        if (currentState == EnemyState.Frozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                Unfreeze();
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBehavior(distanceToPlayer);
                break;
            case EnemyState.Patrolling:
                PatrolBehavior(distanceToPlayer);
                break;
            case EnemyState.Chasing:
                ChaseBehavior(distanceToPlayer);
                break;
        }
    }

    public virtual void TakeDamage(int damage, Vector2 knockbackDirection = default, float knockbackForce = 0)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {health}");

        if (knockbackForce > 0 && knockbackDirection != Vector2.zero)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public event Action OnDeath;
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void IdleBehavior(float distanceToPlayer)
    {
        currentState = EnemyState.Patrolling;
    }

    protected virtual void PatrolBehavior(float distanceToPlayer)
    {
        if (patrolPauseTimer > 0)
        {
            patrolPauseTimer -= Time.deltaTime;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoint, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            patrolPauseTimer = patrolPauseDuration;
            SetNewPatrolPoint();
        }

        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    protected virtual void ChaseBehavior(float distanceToPlayer)
    {
        // Each class will provide his logic
    }

    protected virtual void SetNewPatrolPoint()
    {
        Vector2 randomDirection;
        do
        {
            randomDirection = directions[UnityEngine.Random.Range(0, directions.Length)];
        } while (randomDirection == lastDirection);

        lastDirection = randomDirection;
        patrolPoint = (Vector2)transform.position + randomDirection * UnityEngine.Random.Range(1f, detectionRange / 2f);
    }

    public virtual void Freeze(float duration)
    {
        if (currentState != EnemyState.Frozen)
        {
            currentState = EnemyState.Frozen;
            freezeTimer = duration;
            rb.linearVelocity = Vector2.zero;
            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    protected virtual void Unfreeze()
    {
        currentState = EnemyState.Idle;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerLife = collision.gameObject.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
                Debug.Log($"{gameObject.name} dealt {damage} damage to player.");
            }
        }
    }
}
