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
        if (isOnWater)
        {
            transform.position = lastSafePosition;
            isOnWater = false;
            rb.velocity = Vector2.zero;
            return;
        }

        if (!isOnWater)
        {
            lastSafePosition = transform.position;
        }

        Vector2 moveVelocity = isUsingItem ? Vector2.zero : movement * moveSpeed;

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
