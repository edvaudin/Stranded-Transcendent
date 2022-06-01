using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] BaseStat baseStat;
    public float Value { get; private set; }
    public Action<float> valueChanged;

    public void SetValue(float newValue)
    {
        Value = Mathf.Clamp(newValue, baseStat.MinValue, baseStat.MaxValue);
        valueChanged?.Invoke(Value);
    }

    public void AdjustValue(float delta)
    {
        Value = Mathf.Clamp(Value + delta, baseStat.MinValue, baseStat.MaxValue);
        var newValue = Value;
        valueChanged?.Invoke(Value);
    }

    public void SetValueToBase()
    {
        Value = baseStat.BaseValue;
        valueChanged?.Invoke(Value);
    }
}
