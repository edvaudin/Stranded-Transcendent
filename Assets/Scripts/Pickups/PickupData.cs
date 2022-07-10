using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new pickup data")]
public class PickupData : ScriptableObject
{
    public string displayName;
    public string description;
    public Sprite icon;
    public float dropRate;
}

