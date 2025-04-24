using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Patrolling, Chasing, Attacking, Frozen }
    protected EnemyState currentState;

    public int health;
    public float speed;
    public int damage;
    public float detectionRange;
    public float attackRange;
    public float patrolSpeed;
    public float attackCooldown;
    public float patrolPauseDuration = 2f;
    public float freezeDuration = 3f;

    protected float attackTimer;
    protected Transform player;

    private Vector2 patrolPoint;
    private Vector2 lastDirection;
    private float patrolPauseTimer;
    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    
    private float freezeTimer;

    protected virtual void Start()
    {
        currentState = EnemyState.Idle;
        attackTimer = 0;
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
            case EnemyState.Attacking:
                AttackBehavior(distanceToPlayer);
                break;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ". Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public event Action OnDeath;
    protected virtual void Die()
    {
        Debug.Log("Enemy died!");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void IdleBehavior(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }
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
        // Comportement à définir dans les sous-classes
    }

    protected virtual void AttackBehavior(float distanceToPlayer)
    {
        // Comportement à définir dans les sous-classes
    }

    protected void SetNewPatrolPoint()
    {
        Vector2 randomDirection;
        do
        {
            randomDirection = directions[UnityEngine.Random.Range(0, directions.Length)];
        } while (randomDirection == lastDirection);

        lastDirection = randomDirection;
        patrolPoint = (Vector2)transform.position + randomDirection * UnityEngine.Random.Range(1f, detectionRange / 2f);
    }

    public void Freeze(float duration)
    {
        if (currentState != EnemyState.Frozen)
        {
            currentState = EnemyState.Frozen;
            freezeTimer = duration;

            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    private void Unfreeze()
    {
        currentState = EnemyState.Idle;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerLife>().TakeDamage(damage);
            Debug.Log("Enemy dealt " + damage + " damage to the player.");
        }
    }
}
