using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    public Animator animator;
    public Transform cameraTransform;
    public float attackRange = 2.0f;

    private bool isAttacking = false;
    private PlayerStats playerStats; // ✅ 플레이어 스탯 참조

    void Start()
    {
        // ✅ PlayerStats 자동 할당
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("❌ PlayerStats를 찾을 수 없습니다. MeleeAttack이 플레이어에 연결되어 있는지 확인하세요.");
        }

        // ✅ Animator 자동 할당
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("❌ Animator를 찾을 수 없습니다! Animator를 플레이어에 추가했는지 확인하세요.");
            }
        }
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            return;
        }

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
        animator.SetBool("New Bool", true);
        Debug.Log("⚔️ 공격 실행");
    }

    // ✅ 타격 판정 수행 (애니메이션이 끝날 때 호출됨)
    public void ApplyDamage()
    {
        if (playerStats == null) return; // ✅ PlayerStats가 없으면 실행하지 않음
        int attackDamage = (int)playerStats.AttackPower; // ✅ 현재 플레이어 공격력 사용

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, attackRange, 
            targetLayers))
        {
            Debug.Log(hit.collider.name);

            GameObject targetObject = hit.collider.gameObject;
            int layerIdx = targetObject.layer;
            string layerName = LayerMask.LayerToName(layerIdx);

            if (targetObject.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(attackDamage);
                Debug.Log($"🎯 {targetObject.name}({layerName})에게 {attackDamage} 피해를 입힘!");

                // ✅ 타격 성공 시 타격 소리 재생 (PlayScheduled 활용)

                SoundManager.Instance.PlaySFX(layerName == "Enemy" ? "EnemyAttack" : "WoodAttack");
            }
            else
            {
                SoundManager.Instance.PlaySFX("BaseAttack");
            }
        }
        else
        {
            SoundManager.Instance.PlaySFX("BaseAttack");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}


