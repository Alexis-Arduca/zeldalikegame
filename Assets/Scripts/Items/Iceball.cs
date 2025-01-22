using System.Collections;
using UnityEngine;

public class Iceball : MonoBehaviour
{
    public float speed = 3f;
    public float freezeDelay = 2f;
    private Vector2 direction;
    private bool directionSelect = false;

    private bool hasExploded = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        StartCoroutine(FreezeAfterDelay());
    }

    void Update()
    {
        if (!directionSelect)
        {
            direction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetLastDirection();
            directionSelect = true;
        }

        if (!hasExploded)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private IEnumerator FreezeAfterDelay()
    {
        yield return new WaitForSeconds(freezeDelay);
        Freeze();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasExploded)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit!");
                Enemy enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Freeze(3f);
                }
            }
            else if (collision.CompareTag("Wall"))
            {
                Debug.Log("Wall hit!");
            }

            Freeze();
        }
    }

    private void Freeze()
    {
        if (hasExploded) return;

        hasExploded = true;

        Destroy(gameObject);
    }
}
