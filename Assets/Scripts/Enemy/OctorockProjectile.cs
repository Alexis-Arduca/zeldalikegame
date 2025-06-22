using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OctorockProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerLife = collision.gameObject.GetComponent<PlayerLife>();

            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
                Debug.Log($"OctorockProjectile dealt {damage} damage to player.");
            }
            Destroy(gameObject);
        }
    }
}
