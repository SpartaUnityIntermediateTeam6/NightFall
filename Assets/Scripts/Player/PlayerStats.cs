using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private float _hp;
    [SerializeField] private float _maxSanity = 100f;
    [SerializeField] private float _sanity;

    [Header("이동 관련 스탯")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 7f;

    [Header("공격력 스탯")]
    [SerializeField] private float _attackPower = 10f; // ✅ 기본 공격력 (인스펙터에 표시)
    [SerializeField] private float _meleeAttackPower = 15f; // ✅ 근접 공격력 (인스펙터에 표시)

    // 정신력 감소 관련 변수
    [SerializeField] private float sanityDecayRate = 1f;
    private bool isSanityDecreasing = true;

    // Event Channel
    [SerializeField] private BoundedValueGameEvent hpEventChannel;
    [SerializeField] private BoundedValueGameEvent sanityEventChannel;

    private void Start()
    {
        _sanity = _maxSanity;
        StartCoroutine(DecreaseSanityOverTime());
    }

    public float Hp
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, _maxHp);
            hpEventChannel?.Raise(new BoundedValue(_hp, 0, _maxHp));
        }
    }

    public float Sanity
    {
        get => _sanity;
        set
        {
            _sanity = Mathf.Clamp(value, 0, _maxSanity);
            sanityEventChannel?.Raise(new BoundedValue(_sanity, 0, _maxSanity));

            if (_sanity <= 0)
            {
                OnSanityDepleted();
            }
        }
    }

    public float MoveSpeed => _moveSpeed;
    public float JumpPower => _jumpPower;

    public float AttackPower => _attackPower; // ✅ 인스펙터에서 조절 가능
    public float MeleeAttackPower => _meleeAttackPower; // ✅ 인스펙터에서 조절 가능

    // 🎯 기본 공격력 강화
    public void UpgradeAttackPower(float amount)
    {
        _attackPower += amount;
        Debug.Log($"⚔️ 기본 공격력이 {amount} 만큼 증가! 현재 기본 공격력: {_attackPower}");
    }

    // 🎯 근접 공격력 강화
    public void UpgradeMeleeAttackPower(float amount)
    {
        _meleeAttackPower += amount;
        Debug.Log($"🔪 근접 공격력이 {amount} 만큼 증가! 현재 근접 공격력: {_meleeAttackPower}");
    }

    private IEnumerator DecreaseSanityOverTime()
    {
        while (isSanityDecreasing)
        {
            yield return new WaitForSeconds(1f);
            Sanity -= sanityDecayRate;
        }
    }

    private void OnSanityDepleted()
    {
        Debug.Log("⚠ 플레이어 정신력 소진! 패널티 적용!");
        _moveSpeed = 2f;
    }

    public void RecoverSanity(float amount)
    {
        Sanity += amount;
        Debug.Log($"🧠 정신력 {amount} 회복! 현재 정신력: {_sanity}");
    }

    public void StopSanityDecay()
    {
        isSanityDecreasing = false;
    }

    public void ResumeSanityDecay()
    {
        isSanityDecreasing = true;
        StartCoroutine(DecreaseSanityOverTime());
    }
}

