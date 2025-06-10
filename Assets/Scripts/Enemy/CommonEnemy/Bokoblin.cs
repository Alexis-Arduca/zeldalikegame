using UnityEngine;

public class Bokoblin : Enemy
{
    public float retreatDistance = 2f;
    public float safeDistance = 3f;
    public float attackRange = 1.5f;
    public Transform swordHitbox;
    public float knockbackForce = 2f;
    public float speed = 2f;
    private float stunDuration = 2f;

    private Animator animator;
    private float attackTimer;
    private float attackCooldown = 1.5f;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component missing on Bokoblin!");
        }
        if (swordHitbox == null)
        {
            Debug.LogWarning("SwordHitbox not assigned on Bokoblin!");
        }
        else
        {
            swordHitbox.gameObject.SetActive(false);
        }
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
        UpdateAnimation();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    protected override void ChaseBehavior(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.Patrolling;
        }
        else if (distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            Attack();
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

    private void Attack()
    {
        if (swordHitbox == null || player == null) return;

        animator.SetTrigger("Attack");
        swordHitbox.gameObject.SetActive(true);
        Vector2 direction = (player.position - transform.position).normalized;
        swordHitbox.position = transform.position + (Vector3)direction * 0.5f;
        Invoke(nameof(DeactivateHitbox), 0.3f);
        attackTimer = attackCooldown;
    }

    private void DeactivateHitbox()
    {
        if (swordHitbox != null)
        {
            swordHitbox.gameObject.SetActive(false);
        }
    }

    private void UpdateAnimation()
    {
        if (currentState == EnemyState.Frozen)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        Vector2 velocity = Vector2.zero;
        if (currentState == EnemyState.Chasing)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < retreatDistance)
            {
                velocity = (transform.position - player.position).normalized;
            }
            else if (distanceToPlayer > safeDistance)
            {
                velocity = (player.position - transform.position).normalized;
            }
        }
        else if (currentState == EnemyState.Patrolling)
        {
            velocity = (patrolPoint - (Vector2)transform.position).normalized;
        }

        animator.SetFloat("MoveX", velocity.x);
        animator.SetFloat("MoveY", velocity.y);
        animator.SetBool("IsMoving", velocity != Vector2.zero);

        if (velocity.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (velocity.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Vector2 knockbackDirection = (transform.position - player.position).normalized;
            TakeDamage(10, knockbackDirection, knockbackForce);
            Freeze(stunDuration);
        }
    }

    protected override void Unfreeze()
    {
        base.Unfreeze();
        attackTimer = 0;
        if (swordHitbox != null)
        {
            swordHitbox.gameObject.SetActive(false);
        }
    }
}
