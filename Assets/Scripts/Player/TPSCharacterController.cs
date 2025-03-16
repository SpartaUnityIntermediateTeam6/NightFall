using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCharacterController : MonoBehaviour
{
    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public float coyoteTime = 0.2f;
    public float jumpCooldown = 0.1f;
    [SerializeField] private float groundCastDistance = 0.1f;

    [Header("Gravity Settings")]
    public float gravity = -20f;
    public float fallMultiplier = 2.5f;

    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform; // FPS 카메라 위치

    private float _verticalSpeed = 0f;
    private float _jumpForce;
    private float _lastJumpTime;
    private float _coyoteTimeCounter = 0f;
    private bool _jumpRequested = false;

    void Update()
    {
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
            _verticalSpeed = -2f;
        }
        else
        {
            if (_verticalSpeed < 0)
            {
                _verticalSpeed += gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
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

    bool IsGrounded()
    {
        return Physics.CheckCapsule(transform.position, transform.position + -transform.up * groundCastDistance, controller.radius, groundLayer);
    }
}






