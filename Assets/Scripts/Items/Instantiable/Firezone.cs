using System.Collections;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float duration = 3f;
    public int damage = 5;
    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            int damageToApply = Mathf.RoundToInt(damage * Time.deltaTime);
            collision.GetComponent<Enemy>()?.TakeDamage(damageToApply);
        }
    }

}
