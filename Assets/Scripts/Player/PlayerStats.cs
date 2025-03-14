using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    private float _maxHp;
    private float _hp;
    private float _maxSanity;
    private float _sanity;

    //Event Channel
    [SerializeField] private BoundedValueGameEvent hpEventChannel;
    [SerializeField] private BoundedValueGameEvent sanityEventChannel;

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
        }
    }
}