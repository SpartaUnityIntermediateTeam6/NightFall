using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FPSCinemachineCamera : MonoBehaviour
{
    public Transform playerBody; // 플레이어 본체
    public CinemachineVirtualCamera virtualCamera;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private CinemachineComposer composer;

    void Start()
    {
        // 마우스 커서 숨기고 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Cinemachine Virtual Camera의 Composer 설정 가져오기
        if (virtualCamera != null)
        {
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        }
    }

    void Update()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 상하 회전 (머리 회전)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Cinemachine의 Composer를 활용한 상하 움직임 적용
        if (composer != null)
        {
            composer.m_TrackedObjectOffset.y = xRotation;
        }

        // 좌우 회전 (몸 전체 회전)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

