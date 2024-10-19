using UnityEngine;

public class Target : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isHit = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && (collision.CompareTag("Arrow") || collision.CompareTag("Boomerang")))
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.blue;
            }

            GameObject door = GameObject.FindGameObjectWithTag("Door");
            if (door != null)
            {
                Destroy(door);
            }

            isHit = true;

            Destroy(collision.gameObject);
        }
    }
}
