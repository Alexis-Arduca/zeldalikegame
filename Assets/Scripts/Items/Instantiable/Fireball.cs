using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 3f;
    public float explosionDelay = 2f;
    public GameObject fireZonePrefab;
    private Vector2 direction;
    private bool directionSelect = false;

    private bool hasExploded = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    void Update()
    {
        if (!directionSelect) {
            direction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetLastDirection();
            directionSelect = true;
        }

        if (!hasExploded)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasExploded)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit!");
                collision.GetComponent<Enemy>()?.TakeDamage(10);
            }
            else if (collision.CompareTag("Wall"))
            {
                Debug.Log("Wall hit!");
            }

            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        Instantiate(fireZonePrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
