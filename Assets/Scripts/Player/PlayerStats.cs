using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour, IDamageable
{
    private float _maxHp;
    [SerializeField] private float _hp;
    private float _maxSanity; // 최대 정신력 기본값 추가
    [SerializeField] private float _sanity;
    //Modifier Stats
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 7f;

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
                OnSanityDepleted(); // 정신력 소진 시 패널티 적용
            }
        }
    }

    public float MoveSpeed => _moveSpeed;
    public float JumpPower => _jumpPower;

    // 정신력 감소 관련 변수
    [SerializeField] private float sanityDecayRate = 1f; // 초당 정신력 감소량
    private bool isSanityDecreasing = true; // 정신력 감소 활성화 여부

    // Event Channel
    [SerializeField] private BoundedValueGameEvent hpEventChannel;
    [SerializeField] private BoundedValueGameEvent sanityEventChannel;
    [SerializeField] private BoolGameEvent deadEventChannel;

    private void Awake()
    {
        _maxHp = Hp;
        hpEventChannel?.Raise(new BoundedValue(_hp, 0, _maxHp));

        _maxSanity = Sanity;
        sanityEventChannel?.Raise(new BoundedValue(_sanity, 0, _maxSanity));
    }

    private void Start()
    {
        // 정신력 초기화
        
        StartCoroutine(DecreaseSanityOverTime()); // 정신력 감소 루틴 시작
    }

    public void TakeDamage(float damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);

        if (Hp <= 0) Dead();
    }

    [ContextMenu("Dead")]
    public void Dead()
    {
        Debug.Log("죽음");
        //이벤트 채널 이용
        deadEventChannel?.Raise(false);
    }

    // 🎯 시간에 따라 정신력 감소
    private IEnumerator DecreaseSanityOverTime()
    {
        while (isSanityDecreasing)
        {
            yield return new WaitForSeconds(1f); // 1초마다 감소
            Sanity -= sanityDecayRate;
        }
    }

    // ⚠ 정신력이 0이 되었을 때 패널티 적용 (예: 속도 감소, 화면 흔들림)
    private void OnSanityDepleted()
    {
        Debug.Log("⚠ 플레이어 정신력 소진! 패널티 적용!");
        // 추가 패널티 효과를 여기에 추가 가능
        _moveSpeed = 2f; // 정신력 0이 되면 이동 속도 감소
    }

    // 💊 정신력을 회복하는 함수 (아이템, 휴식 등으로 회복 가능)
    public void RecoverSanity(float amount)
    {
        Sanity += amount;
        Debug.Log($"🧠 정신력 {amount} 회복! 현재 정신력: {_sanity}");
    }

    // 정신력 감소 중단 (예: 특정 이벤트 중)
    public void StopSanityDecay()
    {
        isSanityDecreasing = false;
    }

    // 정신력 감소 다시 시작
    public void ResumeSanityDecay()
    {
        isSanityDecreasing = true;
        StartCoroutine(DecreaseSanityOverTime());
    }
}
