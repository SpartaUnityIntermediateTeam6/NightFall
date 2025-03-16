using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator; // 애니메이션 컨트롤러
    public Transform cameraTransform; // FPS 카메라 위치 (1인칭 기준)
    public float attackRange = 2.0f; // 공격 사거리
    public int attackDamage = 25; // 공격력
    public bool isAttacking = false; // 공격 중인지 체크


    void Start()
    {
        if (animator == null) // animator가 비어있다면 자동으로 할당
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) // 마우스 클릭 (공격)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // 애니메이션 실행
    }

    // 애니메이션 이벤트(Animation Event)에서 호출할 함수 (공격 판정)
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

    // 공격 종료 시 호출될 함수 (애니메이션 이벤트에서 실행)
    public void EndAttack()
    {
        isAttacking = false;
    }

    // Gizmos를 사용하여 공격 범위 확인
    void OnDrawGizmosSelected()
    {
        if (cameraTransform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * attackRange);
    }
}


