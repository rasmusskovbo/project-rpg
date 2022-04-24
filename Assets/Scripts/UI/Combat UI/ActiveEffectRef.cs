using System;
using UnityEngine;

public class ActiveEffectRef : MonoBehaviour
{
    [SerializeField] private CombatEffect reference;

    public CombatEffect Reference
    {
        get => reference;
        set => reference = value;
    }
}
