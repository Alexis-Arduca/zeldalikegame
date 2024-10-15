using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.down;

        animator.SetBool("isMoving", false);
    }

    public void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (movement.x != 0 || movement.y != 0) {
            animator.SetBool("isMoving", true);
            lastDirection = movement.normalized;
        }
        else {
            animator.SetFloat("lastX", lastDirection.x);
            animator.SetFloat("lastY", lastDirection.y);
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("xVelocity", movement.x);
        animator.SetFloat("yVelocity", movement.y);

        rb.velocity = movement;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
}
