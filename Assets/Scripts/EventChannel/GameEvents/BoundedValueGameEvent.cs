using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EventChannel/BoundedValueEvent")]
public class BoundedValueGameEvent : GameEvent<BoundedValue> { }

public class BoundedValue
{
    private float _current;
    private float _min;
    private float _max;

    public float Value
    {
        get => _current;
        set
        {
            _current = Mathf.Clamp(value, _min, _max);
        }
    }

    public float Min => _min;
    public float Max => _max;
    public float Ratio => _current / _max;

    public BoundedValue(float current, float min, float max)
    {
        _min = min;
        _max = max;
        Value = current;
    }

    public BoundedValue(float min, float max)
    {
        _min = min;
        _max = max;
        Value = 0;
    }
}