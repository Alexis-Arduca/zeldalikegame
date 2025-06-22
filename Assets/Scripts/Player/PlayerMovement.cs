using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float shieldSpeedReduction = 0.5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 lastDirection;
    private bool isUsingItem;
    private bool isOnWater;
    private Vector2 lastSafePosition;
    private bool isShieldActive;
    private Vector2 shieldDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.down;
        isUsingItem = false;
        isOnWater = false;
        isShieldActive = false;
        lastSafePosition = transform.position;
        animator.SetBool("isMoving", false);
    }

    public void HandleMovement(Vector2 movement)
    {
        if (isOnWater)
        {
            transform.position = lastSafePosition;
            isOnWater = false;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!isOnWater)
        {
            lastSafePosition = transform.position;
        }

        float currentSpeed = isShieldActive ? moveSpeed * shieldSpeedReduction : moveSpeed;
        Vector2 moveVelocity = isUsingItem ? Vector2.zero : movement * currentSpeed;

        if (moveVelocity != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            if (!isShieldActive)
            {
                lastDirection = movement.normalized;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);

            Vector2 directionToUse = isShieldActive ? shieldDirection : lastDirection;
            animator.SetFloat("lastX", directionToUse.x);
            animator.SetFloat("lastY", directionToUse.y);
        }

        animator.SetFloat("xVelocity", moveVelocity.x);
        animator.SetFloat("yVelocity", moveVelocity.y);
        rb.linearVelocity = moveVelocity;
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

    public void SetShieldState(bool active, Vector2 direction)
    {
        isShieldActive = active;
        shieldDirection = direction;

        if (isShieldActive)
        {
            animator.SetFloat("lastX", shieldDirection.x);
            animator.SetFloat("lastY", shieldDirection.y);
        }
    }
}
