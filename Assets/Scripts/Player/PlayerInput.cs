using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector2> OnMoveInput { get; set; }
    public Action OnAttackInput { get; set; }
    public Action OnUseLeftItemInput { get; set; }
    public Action OnUseRightItemInput { get; set; }
    public Action<int> OnDebugPotionInput { get; set; }
    public Action OnDebugTakeDamageInput { get; set; }
    public Action<bool> OnShieldInput { get; set; }

    public Vector2 MoveDirection { get; private set; }
    public Vector2 AttackDirection { get; private set; }

    void Update()
    {
        HandleMovementInput();
        HandleAttackInput();
        HandleItemInput();
        HandleShieldInput();
        HandleDebugInput();
    }

    private void HandleMovementInput()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        OnMoveInput?.Invoke(MoveDirection);
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackDirection = GetAttackDirection();
            OnAttackInput?.Invoke();
        }
    }

    private Vector2 GetAttackDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow)) return Vector2.up;
        if (Input.GetKey(KeyCode.DownArrow)) return Vector2.down;
        if (Input.GetKey(KeyCode.LeftArrow)) return Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow)) return Vector2.right;
        return Vector2.right;
    }

    private void HandleItemInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnUseLeftItemInput?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnUseRightItemInput?.Invoke();
        }
    }

    private void HandleShieldInput()
    {
        OnShieldInput?.Invoke(Input.GetKey(KeyCode.B));
    }

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Z)) OnDebugPotionInput?.Invoke(1);
        if (Input.GetKeyDown(KeyCode.X)) OnDebugPotionInput?.Invoke(2);
        if (Input.GetKeyDown(KeyCode.C)) OnDebugPotionInput?.Invoke(3);
        if (Input.GetKeyDown(KeyCode.N)) OnDebugTakeDamageInput?.Invoke();
    }
}
