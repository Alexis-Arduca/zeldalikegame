using UnityEngine;

public class Bokoblin : Enemy
{
    public float retreatDistance;
    public float safeDistance;
    public Transform swordHitbox;
    public float knockbackForce;

    private Animator animator;

    private bool isStunned = false;
    private float stunDuration = 2.0f;
    private float stunTimer = 0.0f;

    protected override void Start()
    {
        base.Start();
        health = 50;
        speed = 2f;
        damage = 10;
        detectionRange = 5f;
        attackRange = 1.5f;
        retreatDistance = 2f;
        safeDistance = 3f;
        knockbackForce = 2f;

        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false; 
            }
            return;
        }

        base.Update();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 velocity = (player.position - transform.position).normalized;

        if (currentState == EnemyState.Chasing || currentState == EnemyState.Patrolling)
        {
            animator.SetFloat("MoveX", velocity.x);
            animator.SetFloat("MoveY", velocity.y);
            animator.SetBool("IsMoving", true);

            if (velocity.x < 0)
            {
                transform.localScale = new Vector3(-2, 2, 2);
            }
            else if (velocity.x > 0)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    protected override void ChaseBehavior(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.Patrolling;
        }
        else if (distanceToPlayer < retreatDistance)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, speed * Time.deltaTime);
        }
        else if (distanceToPlayer > safeDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            Vector2 knockbackDirection = (transform.position - player.position).normalized;
            transform.position += (Vector3)knockbackDirection * knockbackForce;

            TakeDamage(10);
        }
    }

    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunDuration = duration;
        stunTimer = stunDuration;
        Debug.Log("Enemy is stunned for " + duration + " seconds.");
    }
}
