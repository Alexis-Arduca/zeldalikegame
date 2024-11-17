using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Patrolling, Chasing, Attacking }
    protected EnemyState currentState;

    public int health;
    public float speed;
    public int damage;
    public float detectionRange;
    public float attackRange;
    public float patrolSpeed;
    public float attackCooldown;
    public float patrolPauseDuration = 2f; // Temps de pause entre les déplacements

    protected float attackTimer;
    protected Transform player;

    private Vector2 patrolPoint;
    private Vector2 lastDirection;
    private float patrolPauseTimer;
    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    protected virtual void Start()
    {
        currentState = EnemyState.Idle;
        attackTimer = 0;
        
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

    protected virtual void Die()
    {
        Debug.Log("Enemy died!");
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
            return; // Pas de déplacement pendant le temps de pause
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoint, patrolSpeed * Time.deltaTime);

        // Vérifier si l'ennemi est arrivé à son point de patrouille
        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            patrolPauseTimer = patrolPauseDuration; // Déclencher le temps de pause
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
        // Choisir une nouvelle direction aléatoire différente de la précédente
        Vector2 randomDirection;
        do
        {
            randomDirection = directions[Random.Range(0, directions.Length)];
        } while (randomDirection == lastDirection); // Éviter les répétitions directes

        lastDirection = randomDirection;
        patrolPoint = (Vector2)transform.position + randomDirection * Random.Range(1f, detectionRange / 2f);
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
