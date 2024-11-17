using UnityEngine;

public class LongshotProjectile : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 direction;
    private GameObject player;
    private PlayerMovement playerMovement;

    private bool isRetracting = false;

    public void Initialize(GameObject player)
    {
        this.player = player;

        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            direction = playerMovement.GetLastDirection();
            playerMovement.UsingItem();
        }
    }

    void Update()
    {
        if (isRetracting && player != null)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, transform.position) < 0.1f)
            {
                playerMovement.UsingItem();
                DestroyLongshot();
            }
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wood") || collision.CompareTag("Chest"))
        {
            isRetracting = true;
            speed = 10f;
        }
        else if (collision.CompareTag("Wall"))
        {
            playerMovement.UsingItem();
            DestroyLongshot();
        }
        else if (collision.CompareTag("Enemy"))
        {
            Bokoblin enemy = collision.GetComponent<Bokoblin>();
            if (enemy != null)
            {
                float stunDuration = 2.0f;
                enemy.ApplyStun(stunDuration);
                Debug.Log("Ennemi étourdi par le boomerang : " + collision.name);
            }
        }
    }

    private void DestroyLongshot()
    {
        Longshot.ResetLongshotInstance();
        Destroy(gameObject);
    }
}
