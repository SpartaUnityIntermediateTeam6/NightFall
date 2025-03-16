using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator; // Animator 연결 필드
    public Transform cameraTransform;
    public float attackRange = 2.0f;
    public int attackDamage = 25;
    private bool isAttacking = false;

    void Start()
    {
        if (animator == null) // Animator 자동 할당
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator를 찾을 수 없습니다! Animator를 플레이어에 추가했는지 확인하세요.");
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (animator == null) return; // Animator가 없으면 실행하지 않음
        isAttacking = true;
        animator.SetTrigger("Attack"); // 애니메이션 실행
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}



