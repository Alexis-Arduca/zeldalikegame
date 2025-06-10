using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private int attackDamage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"Enemy hit! Damage dealt: {attackDamage}");
            }
        }
    }
}
