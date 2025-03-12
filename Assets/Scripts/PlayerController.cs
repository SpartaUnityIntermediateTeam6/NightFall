using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravityMultiplier = 2f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 회전 방지
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        ProcessMovement();
        ApplyExtraGravity();
    }

    private void ProcessMovement()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y).normalized;
        Debug.Log("Move Input: " + input);
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y; // 점프 값 유지
        rb.velocity = velocity;

    }

    private void ApplyExtraGravity()
    {
        if (!isGrounded)
        {
            rb.velocity += Vector3.down * gravityMultiplier * Time.deltaTime;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

