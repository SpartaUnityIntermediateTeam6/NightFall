using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public AttackSoundManager attackSoundManager;
    public Transform cameraTransform;
    public float attackRange = 2.0f;
    public int attackDamage = 25;

    private bool isAttacking = false;

    void Start()
    {
        // Animator 자동 할당
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
        if (animator == null) return; // Animator가 없으면 실행하지 않음
        isAttacking = true;
        animator.SetTrigger("ApplyDamage"); // 공격 애니메이션 실행
        animator.SetBool("New Bool", true); // 공격 애니메이션 실행
        Debug.Log("공격 실행");

        if (attackSoundManager != null)
        {
            attackSoundManager.PlayAttackSound(); // 공격 사운드 재생
        }
    }

    public void ApplyDamage()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, attackRange))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.TakeDamage(attackDamage);
                Debug.Log($"{hit.collider.gameObject.name}에 공격 성공! 체력 감소");
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}





