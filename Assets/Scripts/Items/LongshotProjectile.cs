using UnityEngine;

public class LongshotProjectile : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 direction;
    private GameObject player;

    private bool isRetracting = false;

    public void Initialize(GameObject player)
    {
        this.player = player;

        direction = player.transform.right;
    }

    void Update()
    {
        if (isRetracting && player != null)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isRetracting = true;
            speed = 10f;
        } 
    }
}
