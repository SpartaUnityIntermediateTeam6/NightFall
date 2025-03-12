using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalSpeed = 0f;
    private float gravity = -9.81f;
    private float groundCheckOffset = 0.2f; // 땅 체크 보정값
    private float cameraPitch = 0f;
    private bool isJumping = false;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => TryJump();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    void Update()
    {
        Move();
        RotateCamera();
    }

    void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMove = (forward * moveInput.y + right * moveInput.x) * moveSpeed;

        if (IsGrounded())
        {
            if (isJumping)
            {
                verticalSpeed = jumpForce;
                isJumping = false; // 점프 상태 해제
            }
            else
            {
                verticalSpeed = -0.1f; // 땅에서 바로 중력 영향 최소화
            }
        }
        else
        {
            verticalSpeed += gravity * Time.deltaTime;
        }

        Vector3 moveDirection = desiredMove + Vector3.up * verticalSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void RotateCamera()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -60f, 60f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void TryJump()
    {
        if (IsGrounded())
        {
            isJumping = true;
        }
    }

    bool IsGrounded()
    {
        return controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, controller.bounds.extents.y + groundCheckOffset);
    }
}



