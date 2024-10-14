using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;
    private float attackCooldown = 0.5f;
    private float nextAttackTime = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime)
        {
            StartAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
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

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
