using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public AttackGameEvent attackEventChannel; // 이벤트 채널

    private bool isAttacking = false;

    void Start()
    {
        if (animator == null)
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
        isAttacking = true;
        animator.SetTrigger("Attack"); // 공격 애니메이션 실행
    }

    // 🎯 애니메이션 이벤트에서 호출할 함수 추가 (애니메이션 이벤트에서 실행)
    public void OnAttackEvent()
    {
        if (attackEventChannel != null)
        {
            attackEventChannel.Raise(transform.position); // 이벤트 채널 실행
        }
        else
        {
            Debug.LogWarning("Attack Event Channel이 연결되지 않았습니다.");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}




