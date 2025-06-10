using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerLife = collision.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
                Debug.Log($"EnemyWeapon dealt {damage} damage to player.");
            }
        }

        if (collision.CompareTag("PlayerWeapon"))
        {
            Enemy enemy = GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - collision.transform.position).normalized;
                float knockbackForce = 1.1f;
                enemy.TakeDamage(10, knockbackDirection, knockbackForce);
            }
        }
    }
}
