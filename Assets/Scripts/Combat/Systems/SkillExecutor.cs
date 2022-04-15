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

    private void Awake()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
    }

    /*
     * Handles all active effects that requires action on a unit, each round.
     * Called by the Unit or the system before it's turn.
     */
    public void ProcessAllEffects(Unit unit)
    {
        List<CombatEffect> expiredEffects = new List<CombatEffect>();
        
        unit.ActiveEffects.ForEach(activeEffect =>
        {
            if (activeEffect.Equals(CombatEffectType.Renew))
            {
                unit.Heal(activeEffect.Power);
                activeEffect.DurationTracker.DecreaseDuration();
                if (!activeEffect.DurationTracker.isEffectActive()) expiredEffects.Add(activeEffect);
            }

            // WIP, pseudo code
            if (activeEffect.Equals(CombatEffectType.Poison))
            {
                TakeDamageResult result = unit.SufferDamage(activeEffect.Power);
                combatSystem.CheckForDeath(unit, result);
            }
        });
        
        // done after-the-fact to avoid list manipulation.
        expiredEffects.ForEach(expiredEfect => unit.ActiveEffects.Remove(expiredEfect));
        

    }

    /*
     * Filtering through the different move types.
     * e.g. determining whether it needs a or more targets (mostly offensive)
     * or whether it's a heal or defensive.
     * if it has a special effect such as renew, set's up a self tracking effect on the unit
     */
    public bool ExecuteMove(CombatMove move)
    {
        PlayerCombat player = combatSystem.Player;
        if (move.GetType().Equals(CombatMoveType.Heal))
        {
            // RENEW STYLE
            if (move.GetDuration() > 0)
            {
                combatSystem.Player.AddCombatEffect(move, CombatEffectType.Renew);
                return true;  
            }
            else
            {
                Debug.Log("HEALTH B4: " + combatSystem.Player.CurrentHp);
                player.Heal(move.GetPower());
                Debug.Log("HEALTH AFTER: " + combatSystem.Player.CurrentHp);
                return true;  
            }
            
        }
        
        

        return false;
    }

    
    
    
    
}
