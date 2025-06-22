using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;
    public int damage = 10;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Chest"))
        {
            Destroy(gameObject);
        } else if (collision.CompareTag("Enemy")) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
