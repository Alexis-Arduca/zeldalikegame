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
            rb.linearVelocity = Vector2.zero;
            transform.position = lastSafePosition;
            isOnWater = false;
            return;
        }

        lastSafePosition = transform.position;

        float currentSpeed = isShieldActive ? moveSpeed * shieldSpeedReduction : moveSpeed;
        Vector2 moveVelocity = isUsingItem ? Vector2.zero : movement * currentSpeed;

        bool isMoving = moveVelocity != Vector2.zero;

        animator.SetBool("isMoving", isMoving);
        rb.linearVelocity = moveVelocity;

        if (isMoving)
        {
            if (!isShieldActive)
                lastDirection = movement.normalized;
        }

        Vector2 directionToUse = isShieldActive ? shieldDirection : lastDirection;
        animator.SetFloat("xVelocity", moveVelocity.x);
        animator.SetFloat("yVelocity", moveVelocity.y);
        animator.SetFloat("lastX", directionToUse.x);
        animator.SetFloat("lastY", directionToUse.y);
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
