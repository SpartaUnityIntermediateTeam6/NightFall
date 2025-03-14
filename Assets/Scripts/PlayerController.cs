using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float mouseSensitivity = 2f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public float coyoteTime = 0.2f; // 착지 후 점프 유예 시간
    public float jumpCooldown = 0.1f; // 점프 직후 점프 방지 시간

    [Header("Gravity Settings")]
    public float gravity = -20f;         // 기본 중력 값
    public float fallMultiplier = 2.5f;    // 낙하 시 추가 중력 계수

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalSpeed = 0f;
    private float cameraPitch = 0f;

    private float lastJumpTime;
    private float coyoteTimeCounter = 0f;
    private bool jumpRequested = false;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpRequested = true;

        TestManager.Instance.player = transform;
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
        // 카메라 방향에 기반한 이동 벡터 계산
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMove = (forward * moveInput.y + right * moveInput.x) * moveSpeed;

        bool isGrounded = IsGrounded();

        // 착지 감지 및 코요테 타임 활성화
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 점프 요청 처리
        if (jumpRequested && coyoteTimeCounter > 0f && Time.time - lastJumpTime > jumpCooldown)
        {
            verticalSpeed = jumpForce;
            lastJumpTime = Time.time;
            coyoteTimeCounter = 0f; // 점프 후 코요테 타임 초기화
        }

        jumpRequested = false; // 점프 요청 초기화

        // 중력 적용 (자연스러운 낙하 효과)
        // 점프 후 verticalSpeed가 양수인 경우에는 지면에 닿았더라도 점프 힘을 유지
        if (isGrounded && verticalSpeed <= 0)
        {
            verticalSpeed = -2f; // 지면에 닿은 경우, 충돌 감지를 위해 약간의 하강력 유지
        }
        else
        {
            // 하강 중일 때 추가 중력 적용 (fallMultiplier - 1 만큼 더 가속)
            if (verticalSpeed < 0)
            {
                verticalSpeed += gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
            // 기본 중력 적용
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

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}




