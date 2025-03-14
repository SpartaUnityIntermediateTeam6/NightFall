using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCharacterController : MonoBehaviour
{
    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.3f;
    public float coyoteTime = 0.2f; // 착지 후 점프 유예 시간
    public float jumpCooldown = 0.1f; // 점프 직후 점프 방지 시간
    [SerializeField] private float groundCastDistance = 0.1f;

    [Header("Gravity Settings")]
    public float gravity = -20f;         // 기본 중력 값
    public float fallMultiplier = 2.5f;    // 낙하 시 추가 중력 계수

    [Header("References")]
    public CharacterController controller;

    private float _verticalSpeed = 0f;

    private float _jumpForce;
    private float _lastJumpTime;
    private float _coyoteTimeCounter = 0f;
    private bool _jumpRequested = false;

    void Update()
    {
        ProbingGround();
        HandleJump();
        HandleGravity();
    }

    public void Move(Vector3 desiredMove)
    {
        controller.Move(desiredMove);
    }

    public void Jump(float jumpPower)
    {
        _jumpForce = jumpPower;
        _jumpRequested = true;
    }

    private void HandleGravity()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && _verticalSpeed <= 0)
        {
            _verticalSpeed = -2f; // 지면에 닿은 경우, 충돌 감지를 위해 약간의 하강력 유지
        }
        else
        {
            // 하강 중일 때 추가 중력 적용 (fallMultiplier - 1 만큼 더 가속)
            if (_verticalSpeed < 0)
            {
                _verticalSpeed += gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
            // 기본 중력 적용
            _verticalSpeed += gravity * Time.deltaTime;
        }

        controller.Move(transform.up * _verticalSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        _coyoteTimeCounter = IsGrounded() ? coyoteTime : _coyoteTimeCounter - Time.deltaTime;

        if (_jumpRequested && _coyoteTimeCounter > 0f && Time.time - _lastJumpTime > jumpCooldown)
        {
            _verticalSpeed = _jumpForce;
            _lastJumpTime = Time.time;
            _coyoteTimeCounter = 0f;
        }

        _jumpRequested = false;
    }

    private void ProbingGround()
    {
        //
    }

    public Vector3 CapsuleTopPoint => transform.position + controller.center + Vector3.up * (controller.height / 2 - controller.radius);
    public Vector3 CapsuleBottomPoint => transform.position + controller.center - Vector3.up * (controller.height / 2 - controller.radius);

    bool IsGrounded()
    {
        //Physics.CapsuleCast(controller.bounds.)

        return Physics.CheckCapsule(CapsuleTopPoint, CapsuleBottomPoint + -transform.up * groundCastDistance, controller.radius, groundLayer);
    }
}
