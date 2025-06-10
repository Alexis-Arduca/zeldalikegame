using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 lastDirection;
    private bool isUsingItem;
    private bool isOnWater;
    private Vector2 lastSafePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.down;
        isUsingItem = false;
        isOnWater = false;
        lastSafePosition = transform.position;
        animator.SetBool("isMoving", false);
    }

    public void HandleMovement(Vector2 movement)
    {
        // Si le joueur est sur l'eau, téléportation à la dernière position sûre
        if (isOnWater)
        {
            transform.position = lastSafePosition;
            isOnWater = false;
            rb.velocity = Vector2.zero; // Réinitialiser la vélocité
            return;
        }

        // Mettre à jour la dernière position sûre
        if (!isOnWater)
        {
            lastSafePosition = transform.position;
        }

        // Calculer la vélocité
        Vector2 moveVelocity = isUsingItem ? Vector2.zero : movement * moveSpeed;

        // Mettre à jour l'animation
        if (moveVelocity != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            lastDirection = movement.normalized;
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("lastX", lastDirection.x);
            animator.SetFloat("lastY", lastDirection.y);
        }

        // Appliquer la vélocité
        animator.SetFloat("xVelocity", moveVelocity.x);
        animator.SetFloat("yVelocity", moveVelocity.y);
        rb.velocity = moveVelocity;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }

    public void UsingItem()
    {
        isUsingItem = !isUsingItem;
    }

    public void SetOnWater(bool onWater)
    {
        isOnWater = onWater;
    }
}
