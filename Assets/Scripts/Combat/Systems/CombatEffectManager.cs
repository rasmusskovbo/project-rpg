using System.Collections.Generic;
using UnityEngine;

/*
* Handles all active effects that requires action on a unit, each round.
* Called by the Unit or the system before it's turn.
*/
public class CombatEffectManager : MonoBehaviour
{
    private CombatUnit unit;
    private CombatSystem combatSystem;
    
    private List<CombatEffect> activeEffects;

    private void Awake()
    {
        unit = GetComponent<CombatUnit>();
        combatSystem = FindObjectOfType<CombatSystem>();
        activeEffects = new List<CombatEffect>();
    }

    public void ProcessActiveEffects()
    {
        List<CombatEffect> expiredEffects = new List<CombatEffect>();
        Debug.Log("Unit's active effect list: " + unit.UnitName + ", "+ ActiveEffects.Count);
        
        ActiveEffects.ForEach(activeEffect =>
        {
            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Renew))
            {
                Debug.Log("Applying renew's heal!");
                unit.Heal(activeEffect.Power);
                DecreaseDurationOrExpire(activeEffect, expiredEffects);
                Debug.Log("Remaining duration: " + activeEffect.DurationTracker.GetRemainingDuration());
            }
            
            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Poison))
            {
                TakeDamageResult result = unit.TakeDamage(activeEffect.Power, CombatMoveType.Suffer);
                DecreaseDurationOrExpire(activeEffect, expiredEffects);
                combatSystem.CheckForDeath(result);
            }

            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Block))
            {
                DebugPrintEffects();
                DecreaseDurationOrExpire(activeEffect, expiredEffects);
            }
        });
        
        // done after-the-fact to avoid list manipulation.
        Debug.Log("Expired effects size: " + expiredEffects.Count);
        expiredEffects.ForEach(expiredEffect => ActiveEffects.Remove(expiredEffect));
    }

    private static void DecreaseDurationOrExpire(CombatEffect activeEffect, List<CombatEffect> expiredEffects)
    {
        activeEffect.DurationTracker.DecreaseDuration();
        if (!activeEffect.DurationTracker.isEffectActive()) expiredEffects.Add(activeEffect);
    }

    public bool IsEffectActive(CombatEffectType effect)
    {
        var effectFound = ActiveEffects.Find(activeEffect => activeEffect.CombatEffectType == effect);
        return effectFound != null;
    }
    
    // Active effects
    public float GetEffectMultiplier(CombatEffectType type)
    {
        List<CombatEffect> activeEffectsOfType =
            activeEffects.FindAll(effect => effect.CombatEffectType == type);
        
        float multiplier = 1;
        
        if (activeEffectsOfType.Count == 0)
        {
            return multiplier;
        }
        
        for (int i = 0; i < activeEffectsOfType.Count; i++)
        {
            multiplier = multiplier * (activeEffectsOfType[i].Power / 100) + 1;
        }

        Debug.Log("Multiplier: " + multiplier);
        return multiplier;
    }
    
    public void AddCombatEffect(CombatMove move)
    {
        Debug.Log("Adding " + move.GetEffectType() + " to " + unit.UnitName);
        activeEffects.Add(new CombatEffect(move));
    }
    
    public List<CombatEffect> ActiveEffects
    {
        get => activeEffects;
        set => activeEffects = value;
    }

    public void DebugPrintEffects()
    {
        ActiveEffects.ForEach(effect => Debug.Log("Active effect: " + effect.CombatEffectType));
    }
}
