using System.Collections;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float explosionDelay = 2.0f;
    public float explosionRadius = 2.0f;
    public LayerMask destructibleLayer;

    private void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }

    private void Explode()
    {
        Debug.Log("Bombe explosée !");

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, destructibleLayer);

        foreach (Collider2D obj in hitObjects)
        {
            if (obj.CompareTag("Enemy"))
            {
                Bokoblin enemy = obj.GetComponent<Bokoblin>();
                if (enemy != null)
                {
                    float stunDuration = 2.0f;
                    enemy.Freeze(stunDuration);
                    Debug.Log("Ennemi étourdi : " + obj.name);

                    Vector2 knockbackDirection = (obj.transform.position - transform.position).normalized;
                    float knockbackForce = 3.0f;

                    Rigidbody2D enemyRb = obj.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
            else
            {
                Debug.Log("Objet détruit : " + obj.name);
                Destroy(obj.gameObject);
            }
        }

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
