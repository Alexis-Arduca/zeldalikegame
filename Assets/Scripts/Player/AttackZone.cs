using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public int attackDamage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("Enemy hit! Damage dealt: " + attackDamage);
            }
        }
    }
}
