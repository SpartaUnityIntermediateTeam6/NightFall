using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundManager : MonoBehaviour
{
    public AudioSource attackAudioSource; // 공격 사운드 재생기
    public AudioClip attackSoundClip; // 공격 사운드 클립

    public void PlayAttackSound()
    {
        if (attackAudioSource != null && attackSoundClip != null)
        {
            attackAudioSource.PlayOneShot(attackSoundClip);
        }
        else
        {
            Debug.LogWarning("⚠ 공격 사운드 재생 실패: AudioSource 또는 AudioClip이 설정되지 않음!");
        }
    }
}
