using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    public Transform playerBody; // 플레이어 본체 (좌우 회전)
    public float mouseSensitivity = 100f;

    private float xRotation = 0f; // 위아래 회전 값 (카메라만 회전)
    private Vector2 mouseLook = Vector2.zero; // 마우스 입력값 저장

    private PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Look.performed += ctx => mouseLook = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Look.canceled += ctx => mouseLook = Vector2.zero; // 🎯 마우스 입력이 멈추면 값 초기화
    }

    void Update()
    {
        if (mouseLook == Vector2.zero) return; // 🎯 마우스가 멈췄으면 회전하지 않음

        float mouseX = mouseLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.deltaTime;

        // 🎯 카메라는 상하(x축)만 회전 (위아래 시점 변경)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 시점 제한

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 🎯 플레이어 몸체는 좌우(y축)만 회전 (캐릭터 방향 전환)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}



