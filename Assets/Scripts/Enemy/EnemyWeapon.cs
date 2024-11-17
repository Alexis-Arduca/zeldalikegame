using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerLife>().TakeDamage(damage);
        }

        if (collision.CompareTag("PlayerWeapon"))
        {
            Enemy enemy = GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - collision.transform.position).normalized;
                float knockbackForce = 1.1f;

                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
