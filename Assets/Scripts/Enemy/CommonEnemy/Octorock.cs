using UnityEngine;

public class Octorock : Enemy
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float minShootInterval = 2f;
    public float maxShootInterval = 4f;
    private float shootTimer;
    private bool isShooting;

    protected override void Start()
    {
        base.Start();

        SetNextShootInterval();
        currentState = EnemyState.Patrolling;
    }

    protected override void Update()
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

        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange && !isShooting)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ShootProjectile();
                SetNextShootInterval();
                isShooting = true;
                Invoke(nameof(ResumePatrol), 0.5f);
            }
        }
    }

    private void ResumePatrol()
    {
        isShooting = false;
    }

    protected override void ChaseBehavior(float distanceToPlayer)
    {
        currentState = EnemyState.Patrolling;
    }

    protected override void PatrolBehavior(float distanceToPlayer)
    {
        if (isShooting || patrolPauseTimer > 0)
        {
            patrolPauseTimer -= Time.deltaTime;
            return;
        }

        Vector2 direction = (patrolPoint - (Vector2)transform.position).normalized;
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        direction = absX > absY ? (direction.x > 0 ? Vector2.right : Vector2.left) : (direction.y > 0 ? Vector2.up : Vector2.down);

        transform.position += (Vector3)(direction * patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            transform.position = patrolPoint;
            patrolPauseTimer = patrolPauseDuration;
            SetNewPatrolPoint();
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || player == null)
        {
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        direction = absX > absY ? (direction.x > 0 ? Vector2.right : Vector2.left) : (direction.y > 0 ? Vector2.up : Vector2.down);

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        OctorockProjectile projScript = projectile.GetComponent<OctorockProjectile>();
        if (projScript != null)
        {
            projScript.SetDirection(direction, projectileSpeed);
        }

        UpdateSpriteDirection(direction);
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            if (direction == Vector2.right)
                sprite.flipX = false;
            else if (direction == Vector2.left)
                sprite.flipX = true;
        }
    }

    private void SetNextShootInterval()
    {
        shootTimer = Random.Range(minShootInterval, maxShootInterval);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Vector2 knockbackDirection = (transform.position - player.position).normalized;
            TakeDamage(1, knockbackDirection, 1f);
        }
    }
}
