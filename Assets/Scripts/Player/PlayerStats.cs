using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float _maxHp = 100f;
    private float _hp = 100f;
    private float _maxSanity = 100f;
    private float _sanity = 100f;
    private float _moveSpeed = 5f;
    private float _jumpPower = 7f;

    public float Hp
    {
        get => _hp;
        set => _hp = Mathf.Clamp(value, 0, _maxHp);
    }

    public float Sanity
    {
        get => _sanity;
        set => _sanity = Mathf.Clamp(value, 0, _maxSanity);
    }

    public float MoveSpeed => _moveSpeed;
    public float JumpPower => _jumpPower;
}
