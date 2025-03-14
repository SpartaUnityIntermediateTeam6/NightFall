using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int maxSanity = 100;

    private int currentHP;
    private int currentSanity;

    public event Action<int, int> OnHPChanged; // (현재 HP, 최대 HP)
    public event Action<int, int> OnSanityChanged; // (현재 정신력, 최대 정신력)

    void Start()
    {
        // 초기값 설정
        currentHP = maxHP;
        currentSanity = maxSanity;

        // 초기 UI 업데이트
        OnHPChanged?.Invoke(currentHP, maxHP);
        OnSanityChanged?.Invoke(currentSanity, maxSanity);
    }

    // 플레이어가 피해를 받을 때
    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        OnHPChanged?.Invoke(currentHP, maxHP); // UI 업데이트 호출
    }

    // 정신력 감소
    public void ReduceSanity(int amount)
    {
        currentSanity = Mathf.Max(0, currentSanity - amount);
        OnSanityChanged?.Invoke(currentSanity, maxSanity); // UI 업데이트 호출
    }

    // 체력 회복
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    // 정신력 회복
    public void RestoreSanity(int amount)
    {
        currentSanity = Mathf.Min(maxSanity, currentSanity + amount);
        OnSanityChanged?.Invoke(currentSanity, maxSanity);
    }
}

