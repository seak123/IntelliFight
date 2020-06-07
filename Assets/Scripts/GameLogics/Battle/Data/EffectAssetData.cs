using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SingleTarget,
    AOE,
}
public class EffectAssetData : ScriptableObject
{
    [SerializeField]
    public int ID;

    [SerializeField]
    public EffectType type;
}
