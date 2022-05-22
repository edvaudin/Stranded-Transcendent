using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create stat")]
public class Stat : ScriptableObject
{
    [field: SerializeField] public float BaseValue { get; private set; }
    public float Value { get; private set; }
    [field: SerializeField] public float MinValue { get; private set; }
    [field: SerializeField] public float MaxValue { get; private set; }
    public Action<float> valueChanged;

    public void SetValue(float newValue)
    {
        Value = Mathf.Clamp(newValue, MinValue, MaxValue);
        valueChanged?.Invoke(Value);
    }

    public void AdjustValue(float delta)
    {
        Debug.Log($"Current {this.name} is {Value}");
        Value = Mathf.Clamp(Value + delta, MinValue, MaxValue);
        var newValue = Value;
        Debug.Log($"{this.name} attempted to change by {delta} with clamps {MinValue} - {MaxValue} and changed to {Value}");
        valueChanged?.Invoke(Value);
    }

    public void SetValueToBase()
    {
        Value = BaseValue;
        valueChanged?.Invoke(Value);
    }

    public void SetBaseToValue()
    {
        BaseValue = Value;
    }
}
