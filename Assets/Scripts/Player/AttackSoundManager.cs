using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // 사운드 재생기
    public AudioClip attackSound;    // 공격 소리 (칼 휘두르는 소리)
    public AudioClip hitEnemySound;  // 적 타격 소리
    public AudioClip hitResourceSound; // 자원 채집 소리

    void Start()
    {
        // AudioSource 자동 할당
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    // ⚔️ 공격 버튼을 눌렀을 때 실행 (모션 시작 시)
    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
        else
        {
            Debug.LogWarning("❌ 공격 소리가 설정되지 않았습니다!");
        }
    }

    // 🎯 적중했을 때 실행 (레이어에 따라 다른 소리)
    public void PlayHitSound(GameObject target)
    {
        int targetLayer = target.layer;
        string layerName = LayerMask.LayerToName(targetLayer);

        if (layerName == "Enemy")
        {
            if (hitEnemySound != null)
                audioSource.PlayOneShot(hitEnemySound);
            else
                Debug.LogWarning("❌ 적 타격 소리가 설정되지 않았습니다!");
        }
        else if (layerName == "Resource")
        {
            if (hitResourceSound != null)
                audioSource.PlayOneShot(hitResourceSound);
            else
                Debug.LogWarning("❌ 자원 채집 소리가 설정되지 않았습니다!");
        }
    }
}


