using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private GameObject attackZoneUp;
    [SerializeField] private GameObject attackZoneDown;
    [SerializeField] private GameObject attackZoneLeft;
    [SerializeField] private GameObject attackZoneRight;

    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        DeactivateAllAttackZones();
        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
    }

    private void OnActionChange()
    {
        canAttack = !canAttack;
    }

    public void PerformAttack(Vector2 direction)
    {
        if (!canAttack) return;
        StartCoroutine(AttackCoroutine(direction));
    }

    private IEnumerator AttackCoroutine(Vector2 direction)
    {
        ActivateAttackZone(direction);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDuration);
        DeactivateAllAttackZones();
    }

    private void ActivateAttackZone(Vector2 direction)
    {
        DeactivateAllAttackZones();
        if (direction == Vector2.up) attackZoneUp.SetActive(true);
        else if (direction == Vector2.down) attackZoneDown.SetActive(true);
        else if (direction == Vector2.left) attackZoneLeft.SetActive(true);
        else if (direction == Vector2.right) attackZoneRight.SetActive(true);
    }

    private void DeactivateAllAttackZones()
    {
        attackZoneUp.SetActive(false);
        attackZoneDown.SetActive(false);
        attackZoneLeft.SetActive(false);
        attackZoneRight.SetActive(false);
    }
}
