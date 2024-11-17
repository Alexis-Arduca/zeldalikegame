using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    public GameObject attackZoneUp;
    public GameObject attackZoneDown;
    public GameObject attackZoneLeft;
    public GameObject attackZoneRight;

    public float attackDuration = 0.3f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        attackZoneUp.SetActive(false);
        attackZoneDown.SetActive(false);
        attackZoneLeft.SetActive(false);
        attackZoneRight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        Vector2 attackDirection = GetAttackDirection();

        ActivateAttackZone(attackDirection);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);

        DeactivateAllAttackZones();
    }

    private Vector2 GetAttackDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            return Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow))
            return Vector2.down;
        if (Input.GetKey(KeyCode.LeftArrow))
            return Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow))
            return Vector2.right;

        return Vector2.right;
    }

    private void ActivateAttackZone(Vector2 direction)
    {
        DeactivateAllAttackZones();

        if (direction == Vector2.up)
            attackZoneUp.SetActive(true);
        else if (direction == Vector2.down)
            attackZoneDown.SetActive(true);
        else if (direction == Vector2.left)
            attackZoneLeft.SetActive(true);
        else if (direction == Vector2.right)
            attackZoneRight.SetActive(true);
    }

    private void DeactivateAllAttackZones()
    {
        attackZoneUp.SetActive(false);
        attackZoneDown.SetActive(false);
        attackZoneLeft.SetActive(false);
        attackZoneRight.SetActive(false);
    }
}
