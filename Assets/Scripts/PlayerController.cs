using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f; // 점프 힘 증가
    public float mouseSensitivity = 2f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public float coyoteTime = 0.2f; // 착지 직후 점프할 수 있는 유예 시간

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalSpeed = 0f;
    private float gravity = -20f; // 중력 강화
    private float cameraPitch = 0f;

    private bool jumpPressed = false;
    private float coyoteTimeCounter = 0f;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
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

        // 땅 감지 + 코요테 타임 (착지 직후 점프 가능)
        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // 착지했으므로 코요테 타임 초기화
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // 공중에 있는 경우 감소
        }

        // 점프 처리 (코요테 타임이 남아있을 때만 점프 가능)
        if (jumpPressed && coyoteTimeCounter > 0f)
        {
            verticalSpeed = jumpForce;
            coyoteTimeCounter = 0f; // 점프 후 코요테 타임 제거
            jumpPressed = false; // 점프 입력 초기화
        }

        if (!isGrounded)
        {
            verticalSpeed += gravity * Time.deltaTime; // 중력 적용
        }
        else if (verticalSpeed < 0f)
        {
            verticalSpeed = -2f; // 지면에 있을 때 즉시 멈추도록 설정
        }

        Vector3 moveDirection = desiredMove + Vector3.up * verticalSpeed;
        controller.Move(moveDirection * Time.deltaTime);

        // 점프 입력 초기화
        jumpPressed = false;
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

    bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer).Length > 0;
    }
}




