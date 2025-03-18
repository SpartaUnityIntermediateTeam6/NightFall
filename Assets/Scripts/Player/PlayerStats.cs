using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public SunMoonCycle sunMoonCycle;

    [Header("기본 스탯")]
    [SerializeField] private float _maxHp = 100f;
    private float _hp;
    [SerializeField] private float _maxSanity = 100f;
    private float _sanity;

    [Header("이동 관련 스탯")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 7f;

    [Header("공격력 스탯")]
    [SerializeField] private float _attackPower = 10f; // ✅ 기본 공격력 (인스펙터에 표시)
    [SerializeField] private float _meleeAttackPower = 15f; // ✅ 근접 공격력 (인스펙터에 표시)

    // 정신력 감소 관련 변수
    [SerializeField] private float sanityDecayRate = 1f; // 초당 정신력 감소량
    [SerializeField] private float sanityAddRate = 1f; // 초당 정신력 회복량

    // Event Channel
    [SerializeField] private BoundedValueGameEvent hpEventChannel;
    [SerializeField] private BoundedValueGameEvent sanityEventChannel;
    [SerializeField] private BoolGameEvent deadEventChannel;

    private bool isSanityDecreasing = true;

    Coroutine _coroutine = null;

    public float MoveSpeed => _moveSpeed;
    public float JumpPower => _jumpPower;

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

    private void Awake()
    {
        Hp = _maxHp;
        hpEventChannel?.Raise(new BoundedValue(_hp, 0, _maxHp));

        Sanity = _maxSanity;
        sanityEventChannel?.Raise(new BoundedValue(_sanity, 0, _maxSanity));
    }

    private void Update()
    {
        if (_coroutine == null)
        {
            if (sunMoonCycle.isNight)
            {
                _coroutine = StartCoroutine(DecreaseSanityOverTime());
            }
            else
            {
                _coroutine = StartCoroutine(AddSanityOverTime());
            }
        }
    }

    

    public void TakeDamage(float damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);

        if (Hp <= 0) Dead();
    }

    [ContextMenu("Dead")]
    public void Dead()
    {
        deadEventChannel?.Raise(false);
    }

    // 🎯 시간에 따라 정신력 감소

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

    public IEnumerator DecreaseSanityOverTime()
    {
        while (sunMoonCycle.isNight)
        {
            yield return new WaitForSeconds(1f);
            Sanity -= sanityDecayRate;
        }

        _coroutine = null;
    }

    public IEnumerator AddSanityOverTime()
    {
        while (!sunMoonCycle.isNight)
        {
            yield return new WaitForSeconds(1f);
            Sanity += sanityAddRate;
        }

        _coroutine = null;
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

