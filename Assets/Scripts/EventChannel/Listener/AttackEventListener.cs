using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventListener : MonoBehaviour, IGameEventListener<Vector3>
{
    public AttackGameEvent attackEventChannel; // 이벤트 채널
    public Transform cameraTransform;
    public float attackRange = 2.0f;
    public int attackDamage = 25;

    void OnEnable()
    {
        if (attackEventChannel != null)
            attackEventChannel.RegisterListener(this); // 인터페이스 기반 등록
    }

    void OnDisable()
    {
        if (attackEventChannel != null)
            attackEventChannel.DeregisterListener(this);
    }

    public void OnEventRaised(Vector3 attackPosition)
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
}

