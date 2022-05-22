using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create base stat")]
public class BaseStat : ScriptableObject
{
    [field: SerializeField] public float BaseValue { get; private set; }
    
    [field: SerializeField] public float MinValue { get; private set; }
    [field: SerializeField] public float MaxValue { get; private set; }
}
