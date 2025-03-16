using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class InputReader : PlayerInputActions.IPlayerActions
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool AimInput { get; private set; } // 추가됨

    private PlayerInputActions _playerInputActions;

    public InputReader()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.SetCallbacks(this);
        _playerInputActions.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpInput = context.performed;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        FireInput = context.performed;
    }

    public void OnAim(InputAction.CallbackContext context) // 추가된 부분
    {
        AimInput = context.performed;
    }
}


