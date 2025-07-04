using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector2> OnMoveInput { get; set; }
    public Action OnAttackInput { get; set; }
    public Action OnUseLeftItemInput { get; set; }
    public Action OnUseRightItemInput { get; set; }
    public Action<bool> OnShieldInput { get; set; }

    private PlayerInputAction inputActions;
    
    private const float moveDeadzone = 0.2f;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            OnMoveInput?.Invoke(input.normalized);
        };

        inputActions.Player.Move.canceled += ctx =>
        {
            OnMoveInput?.Invoke(Vector2.zero);
        };


        inputActions.Player.Interact.performed += ctx => GameEventsManager.instance.playerEvents.OnPlayerInteract();
        inputActions.Player.Menu.performed += ctx => GameEventsManager.instance.playerEvents.OnPlayerOpenMenu();

        inputActions.Player.Sword.performed += ctx => OnAttackInput?.Invoke();
        inputActions.Player.Shield.performed += ctx => OnShieldInput?.Invoke(true);
        inputActions.Player.Shield.canceled += ctx => OnShieldInput?.Invoke(false);

        inputActions.Player.LeftItem.performed += ctx => OnUseLeftItemInput?.Invoke();
        inputActions.Player.RightItem.performed += ctx => OnUseRightItemInput?.Invoke();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();
}
