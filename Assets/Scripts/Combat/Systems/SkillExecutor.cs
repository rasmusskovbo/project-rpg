using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    // Filters move by target.
    public List<TakeDamageResult> ExecuteMove(CombatMove move, CombatUnit user, CombatUnit target, List<GameObject> allEnemies = null)
    {
        skillManager.PutCombatMoveOnCooldown(move);
        
        switch (move.GetTargets())
        {
            case CombatMoveTargets.Self:
                ExecuteMoveOnSelf(move, user);
                break;
            case CombatMoveTargets.Adjacent:
                return ExecuteMoveOnMultipleTargets(move, user, target, allEnemies);
            case CombatMoveTargets.Global:
                return ExecuteMoveOnMultipleTargets(move, user, target, allEnemies);
            case CombatMoveTargets.Singular:
                return new List<TakeDamageResult> {ExecuteMoveOnTarget(move, user, target)};
        }
        
        return null;
    }
    
    /*
     * Self target
     */
    public void ExecuteMoveOnSelf(CombatMove move, CombatUnit unit)
    {
        switch (move.GetType())
        {
            case CombatMoveType.Heal:
                ExecuteHealTypeMove(move, unit);
                break;
            case CombatMoveType.Block:
                ExecuteDefendTypeMove(move, unit);
                break;
            case CombatMoveType.Mitigate:
                ExecuteDefendTypeMove(move, unit);
                break;
            case CombatMoveType.Buff:
                AddBuffToUnit(move, unit);
                break;
        }
    }

    /*
     * Global or Adjacent targets
     */
    public List<TakeDamageResult> ExecuteMoveOnMultipleTargets(CombatMove move, CombatUnit attacker, CombatUnit target, List<GameObject> allEnemies)
    {
        List<TakeDamageResult> results = new List<TakeDamageResult>();
        
        if (move.GetTargets() == CombatMoveTargets.Global)
        {
            allEnemies.ForEach(enemyGO =>
            {
                TakeDamageResult result = ExecuteMoveOnTarget(move, attacker, enemyGO.GetComponent<CombatUnit>());
                results.Add(result);
            });
            
            return results;
        }
        else // Adjacent
        {
            combatSystem.GetTargetAndActiveAdjacentPosition(target).ForEach(enemy =>
            {
                TakeDamageResult result =
                    ExecuteMoveOnTarget(move, attacker, enemy);
                results.Add(result);
            });
            
            return results;
        }
    }

    /*
     * Singular Target
     */
    public TakeDamageResult ExecuteMoveOnTarget(CombatMove move, CombatUnit attacker, CombatUnit target)
    {
        if (move.GetType().Equals(CombatMoveType.Debuff))
        {
            return AddDebuffToUnit(move, target);
        }
        
        if (move.GetType().Equals(CombatMoveType.Suffer))
        {
            return ExecuteSufferAttackMove(move, attacker, target);    
        }

        return ExecuteRegularAttackMove(attacker, target, move);
        
    }

    /*
     * Physical or Magical attack
     */
    private TakeDamageResult ExecuteRegularAttackMove(CombatUnit attacker, CombatUnit target, CombatMove move)
    {
        float damage = attacker.DoDamage(move);
        var takeDamageResult = target.TakeDamage(damage, move.GetType());
        combatLog.UsedOffensiveCombatMove(move, attacker, target, takeDamageResult.DamageTaken);
        return takeDamageResult;
    }
    
    /*
     * Suffer attack
     */
    private TakeDamageResult ExecuteSufferAttackMove(CombatMove move, CombatUnit attacker, CombatUnit target)
    {
        // Suffer damage is true damage and does not get multiplied.
        // Sometimes this is called due to a combat effect, log is not correct.
        combatLog.UsedOffensiveCombatMove(move, attacker, target, move.GetPower());
        return target.TakeDamage(move.GetPower(), CombatMoveType.Suffer);
    }
    
    /*
     * Healing
     */
    private void ExecuteHealTypeMove(CombatMove move, CombatUnit unit)
    {
        if (move.GetEffectType().Equals(CombatEffectType.Renew))
        {
            AddBuffToUnit(move, unit);
        }
        else
        {
            combatLog.PlayerHealed(move);
            unit.Heal(move.GetPower());
        }
    }
    
    /*
     * Block or Mitigate
     */
    private void ExecuteDefendTypeMove(CombatMove move, CombatUnit unit)
    {
        if (move.GetEffectType() == CombatEffectType.Block)
        {
            Debug.Log("Added Block");
            
            combatLog.PlayerAppliedBlock(move);

            unit.CurrentPhysicalBlock = move.GetPower();
            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move);
        }
        else
        {
            Debug.Log("Activating mitigation");
            
            combatLog.UnitAppliedEffect(unit, move);

            switch (move.GetEffectType())
            {
                case CombatEffectType.PhysMitigation: 
                    float physMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = physMitigation / 100;
                    break;
                case CombatEffectType.MagicMitigation: 
                    float magicMitigation = move.GetPower();
                    unit.CurrentMagicalMitigation = magicMitigation / 100;
                    break;
                case CombatEffectType.AllMitigation:
                    float allPhysMitigation = move.GetPower();
                    float allMagicMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = allPhysMitigation / 100;
                    unit.CurrentMagicalMitigation = allMagicMitigation / 100;
                    break;
            }
            
            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move);
        }
    }

    /*
     * Apply active beneficial effect to self
     */
    private void AddBuffToUnit(CombatMove buffMove, CombatUnit unit)
    {
        Debug.Log("Buffed with "+ buffMove.GetEffectType() + " effect");
        
        unit.GetComponent<CombatEffectManager>().AddCombatEffect(buffMove);
        combatLog.UnitAppliedEffect(unit, buffMove);
    }

    /*
     * Apply active negative effect to target
     */
    private TakeDamageResult AddDebuffToUnit(CombatMove move, CombatUnit target)
    {
        Debug.Log("Debuffed with "+ move.GetEffectType() + " effect");
        
        target.GetComponent<CombatEffectManager>().AddCombatEffect(move);
        combatLog.UnitAppliedEffect(target, move);

        return new TakeDamageResult(target, false, 0);
    }


}
