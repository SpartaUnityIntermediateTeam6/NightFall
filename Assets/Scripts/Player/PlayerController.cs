using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TPSCharacterController _controller;
    private PlayerStats _stats;
    private InputReader _inputReader;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float aimFOV = 30f; // 조준 시 FOV
    private float defaultFOV;

    private float _xRotation = 0f;
    private Camera _mainCamera;

    void Awake()
    {
        _controller = GetComponent<TPSCharacterController>();
        _stats = GetComponent<PlayerStats>();
        _inputReader = new InputReader();
        _mainCamera = Camera.main;

        defaultFOV = _mainCamera.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        Move(_inputReader.MoveInput);
        if (_inputReader.JumpInput) Jump();
        if (_inputReader.FireInput) Fire();
        if (_inputReader.AimInput) Aim();
        else ResetAim();
    }

    void HandleMouseLook()
    {
        Vector2 lookInput = _inputReader.LookInput;
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void Move(Vector2 moveInput)
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        _controller.Move(moveDirection * Time.deltaTime * _stats.MoveSpeed);
    }

    public void Jump()
    {
        _controller.Jump(_stats.JumpPower);
    }

    public void Fire()
    {
        Debug.Log("총 발사!");
    }

    public void Aim()
    {
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, aimFOV, Time.deltaTime * 10f);
    }

    public void ResetAim()
    {
        _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, defaultFOV, Time.deltaTime * 10f);
    }
}
