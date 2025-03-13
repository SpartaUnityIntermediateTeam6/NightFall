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

    public event Action<int, int> OnHPChanged; // (���� HP, �ִ� HP)
    public event Action<int, int> OnSanityChanged; // (���� ���ŷ�, �ִ� ���ŷ�)

    void Start()
    {
        // �ʱⰪ ����
        currentHP = maxHP;
        currentSanity = maxSanity;

        // �ʱ� UI ������Ʈ
        OnHPChanged?.Invoke(currentHP, maxHP);
        OnSanityChanged?.Invoke(currentSanity, maxSanity);
    }

    // �÷��̾ ���ظ� ���� ��
    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        OnHPChanged?.Invoke(currentHP, maxHP); // UI ������Ʈ ȣ��
    }

    // ���ŷ� ����
    public void ReduceSanity(int amount)
    {
        currentSanity = Mathf.Max(0, currentSanity - amount);
        OnSanityChanged?.Invoke(currentSanity, maxSanity); // UI ������Ʈ ȣ��
    }

    // ü�� ȸ��
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    // ���ŷ� ȸ��
    public void RestoreSanity(int amount)
    {
        currentSanity = Mathf.Min(maxSanity, currentSanity + amount);
        OnSanityChanged?.Invoke(currentSanity, maxSanity);
    }
}

