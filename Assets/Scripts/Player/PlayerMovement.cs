using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking;
    private Vector2 lastDirection;

    private float attackCooldown = 0.5f;
    private float nextAttackTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isAttacking = false;
        lastDirection = Vector2.down;

        animator.SetBool("isMoving", false);
    }

    public void HandleMovement()
    {
        if (!isAttacking)
        {
            movement.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
            movement.y = Input.GetAxisRaw("Vertical") * moveSpeed;

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("isMoving", true);
                lastDirection = movement.normalized;
            }
            else
            {
                animator.SetFloat("lastX", lastDirection.x);
                animator.SetFloat("lastY", lastDirection.y);
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            movement = Vector2.zero;

            animator.SetFloat("lastX", lastDirection.x);
            animator.SetFloat("lastY", lastDirection.y);
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("xVelocity", movement.x);
        animator.SetFloat("yVelocity", movement.y);

        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime)
        {
            StartAttack();
            nextAttackTime = Time.time + attackCooldown;
        }

        rb.velocity = movement;
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
