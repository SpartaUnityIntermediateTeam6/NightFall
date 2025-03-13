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
    public float coyoteTime = 0.2f; // ���� �� ���� ���� �ð�
    public float jumpCooldown = 0.1f; // ���� ���� ���� ���� �ð�

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalSpeed = 0f;
    private float gravity = -20f; // �߷� ����
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

        bool isGrounded = IsGrounded();

        // ���� ���� (�ڿ��� Ÿ�� Ȱ��ȭ)
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // ���� ��û ó��
        if (jumpRequested && coyoteTimeCounter > 0f && Time.time - lastJumpTime > jumpCooldown)
        {
            verticalSpeed = jumpForce;
            lastJumpTime = Time.time;
            coyoteTimeCounter = 0f; // ���� �� �ڿ��� Ÿ�� �ʱ�ȭ
        }

        jumpRequested = false; // ���� ��û �ʱ�ȭ

        // �߷� ����
        if (!isGrounded)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }
        else if (verticalSpeed < 0f)
        {
            verticalSpeed = -2f; // ���鿡�� �ε巴�� ���ߵ��� ����
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




