using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Used for handling execution of different skill types (who's it targetting, defensive, offensive etc)
 */
public class SkillExecutor : MonoBehaviour
{
    private CombatSystem combatSystem;
    private SkillManager skillManager;
    private UICombatLog combatLog;

    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        skillManager = FindObjectOfType<SkillManager>();
        combatLog = FindObjectOfType<UICombatLog>();
    }

    // Take damage result on global hits. maybe always return a list.
    public TakeDamageResult ExecuteMove(CombatMove move, CombatUnit target, List<GameObject> allEnemies = null)
    {
        Debug.Log("Trying to execute move: " + move.GetName() + ", " + move.GetType() + ", " + move.GetEffectType());
        
        switch (move.GetTargets())
        {
            case CombatMoveTargets.Self:
                ExecuteMoveOnSelf(target, move);
                break;
            case CombatMoveTargets.Adjacent:
                ExecuteMoveOnMultipleTargets(target, allEnemies, move);
                break;
            case CombatMoveTargets.Global:
                ExecuteMoveOnMultipleTargets(target, allEnemies, move);
                break;
            case CombatMoveTargets.Singular:
                return ExecuteMoveOnTarget(target, move);
        }

        skillManager.PutCombatMoveOnCooldown(move);
        
        return null;
    }
    
    public void ExecuteMoveOnSelf(CombatUnit player, CombatMove move)
    {
        if (move.GetType().Equals(CombatMoveType.Heal))
        {
            ExecuteHealTypeMove(player, move);
        }
    }

    public void ExecuteMoveOnMultipleTargets(CombatUnit target, List<GameObject> allEnemies, CombatMove move)
    {
        Debug.Log("Executing move on multiple targets");
        // if global = all
        // if adjacent = some
    }

    public TakeDamageResult ExecuteMoveOnTarget(CombatUnit target, CombatMove move)
    {
        // Use on target
        Debug.Log("Executing move on singular target");
        if (move.GetType().Equals(CombatMoveType.Suffer))
        {
            return ExecuteSufferTypeMove(target, move);
        }
        else
        {
            return target.TakeDamage(move.GetPower(), move.GetType());
        }
    }
    
    
    /*
     * Handles all active effects that requires action on a unit, each round.
     * Called by the Unit or the system before it's turn.
     * Refactor the effect handler
     */
    public void ProcessAllEffects(CombatUnit unit)
    {
        List<CombatEffect> expiredEffects = new List<CombatEffect>();
        Debug.Log("Unit's active effect list: " + unit.UnitName + ", "+ unit.ActiveEffects.Count);
        unit.ActiveEffects.ForEach(activeEffect =>
        {
            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Renew))
            {
                Debug.Log("Applying renew's heal!");
                unit.Heal(activeEffect.Power);
                activeEffect.DurationTracker.DecreaseDuration();
                if (!activeEffect.DurationTracker.isEffectActive()) expiredEffects.Add(activeEffect);
                Debug.Log("Remaining duration: " + activeEffect.DurationTracker.GetRemainingDuration());
            }
            
            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Poison))
            {
                TakeDamageResult result = unit.TakeDamage(activeEffect.Power, CombatMoveType.Suffer);
                combatSystem.CheckForDeath(unit, result);
            }
        });
        
        // done after-the-fact to avoid list manipulation.
        Debug.Log("Expired effects size: " + expiredEffects.Count);
        expiredEffects.ForEach(expiredEffect => unit.ActiveEffects.Remove(expiredEffect));
    }

    /*
     * Scripting
     */
    private void ExecuteHealTypeMove(CombatUnit unit, CombatMove move)
    {
        if (move.GetEffectType().Equals(CombatEffectType.Renew))
        {
            Debug.Log("Added Renew effect");
            
            combatLog.PlayerAppliedBuff(move);
            unit.AddCombatEffect(move, CombatEffectType.Renew);
        }
        else
        {
            combatLog.PlayerHealed(move);
            unit.Heal(move.GetPower());
        }
    }
    
    private TakeDamageResult ExecuteSufferTypeMove(CombatUnit target, CombatMove move)
    {
        
        if (move.GetEffectType().Equals(CombatEffectType.Poison))
        {
            Debug.Log("Added Poison effect");
            target.AddCombatEffect(move, CombatEffectType.Poison);
            
            combatLog.PlayerAppliedDebuff(move, target);
            return new TakeDamageResult(false, 0);
        }
        else
        {
            combatLog.PlayerUsedCombatMove(move, target, move.GetPower());
            return target.TakeDamage(move.GetPower(), CombatMoveType.Suffer);
        }
    }
}
