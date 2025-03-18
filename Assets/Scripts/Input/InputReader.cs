using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    private PlayerInputActions _inputActions;
    public PlayerInputActions Input => _inputActions;

    public UnityEvent<Vector2> OnMoveEvent;
    public UnityEvent OnJumpEvent;
    public UnityEvent OnAttackEvent;
    public UnityEvent OnInteractionEvent;
    public UnityEvent OnInventoryEvent;

    void Awake()
    {
        _inputActions = new();
        _inputActions.Player.SetCallbacks(this);
    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        _inputActions.Enable();
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnJumpEvent?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // 버튼이 눌렸을 때만 실행
        {
            OnAttackEvent?.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnInteractionEvent?.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInventoryEvent?.Invoke();
        }
    }
}
