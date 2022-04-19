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

    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        skillManager = FindObjectOfType<SkillManager>();
    }
    
    /*
     * Filtering through the different move types.
     * e.g. determining whether it needs a or more targets (mostly offensive)
     * or whether it's a heal or defensive.
     * if it has a special effect such as renew, set's up a self tracking effect on the unit
     *
     * Should return TRUE if move does not have target selection.  
     */
    public bool ExecuteMove(CombatUnit unit, CombatMove move)
    {
        Debug.Log("Trying to execute move: " + move.GetName() + ", " + move.GetType() + ", " + move.GetEffectType());
        ProcessHealingTypeMoves(unit, move);
        //ProcessBuffMoves
        //ProcessDebuffMoves
        // Skills that needs a target, singular or adjacent, needs different method in target select.
        // (to get adjacent units for dmg)

        return DoesMoveNeedTargets(move);
    }
    
    /*
     * Handles all active effects that requires action on a unit, each round.
     * Called by the Unit or the system before it's turn.
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

            // WIP, pseudo code
            if (activeEffect.CombatEffectType.Equals(CombatEffectType.Poison))
            {
                TakeDamageResult result = unit.TakeDamage(5, CombatMoveType.Suffer);
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
    private bool DoesMoveNeedTargets(CombatMove move)
    {
        if (move.GetTargets().Equals(CombatMoveTargets.Self) || move.GetTargets().Equals(CombatMoveTargets.Global))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ProcessHealingTypeMoves(CombatUnit unit, CombatMove move)
    {
        if (move.GetType().Equals(CombatMoveType.Heal))
        {
            if (move.GetEffectType().Equals(CombatEffectType.Renew))
            {
                Debug.Log("Added Renew effect");
                unit.AddCombatEffect(move, CombatEffectType.Renew);
                skillManager.PutCombatMoveOnCooldown(move);
            }
            else
            {
                unit.Heal(move.GetPower());
                skillManager.PutCombatMoveOnCooldown(move);
            }
        }
    }
}
