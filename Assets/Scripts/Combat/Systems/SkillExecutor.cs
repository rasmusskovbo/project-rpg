using System.Collections.Generic;
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
    public TakeDamageResult ExecuteMove(CombatMove move, CombatUnit user, CombatUnit target, List<GameObject> allEnemies = null)
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
                return ExecuteMoveOnTarget(user,target, move);
        }

        skillManager.PutCombatMoveOnCooldown(move);
        
        return null;
    }
    
    public void ExecuteMoveOnSelf(CombatUnit player, CombatMove move)
    {
        switch (move.GetType())
        {
            case CombatMoveType.Heal:
                ExecuteHealTypeMove(player, move);
                break;
            case CombatMoveType.Block:
                ExecuteDefendTypeMove(player, move);
                break;
            case CombatMoveType.Mitigate:
                ExecuteDefendTypeMove(player, move);
                break;
        }
    }

    // Global or adjacent
    public void ExecuteMoveOnMultipleTargets(CombatUnit target, List<GameObject> allEnemies, CombatMove move)
    {
        Debug.Log("Executing move on multiple targets");
        // if global = all
        // if adjacent = some
    }

    public TakeDamageResult ExecuteMoveOnTarget(CombatUnit attacker, CombatUnit target, CombatMove move)
    {
        // Use on target
        Debug.Log("Executing move on singular target");
        if (move.GetType().Equals(CombatMoveType.Suffer))
        {
            return ExecuteSufferTypeMove(attacker, target, move);
        }
        else
        {
            var takeDamageResult = target.TakeDamage(move.GetPower(), move.GetType());
            combatLog.UsedOffensiveCombatMove(move, attacker, target, takeDamageResult.DamageTaken);
            return takeDamageResult;
        }
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
            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.Renew);
        }
        else
        {
            combatLog.PlayerHealed(move);
            unit.Heal(move.GetPower());
        }
    }
    
    private void ExecuteDefendTypeMove(CombatUnit unit, CombatMove move)
    {
        if (move.GetEffectType() == CombatEffectType.Block)
        {
            Debug.Log("Added Block");
            
            combatLog.PlayerAppliedBlock(move);

            unit.CurrentPhysicalBlock = move.GetPower();
            unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.Block);
        }
        else
        {
            Debug.Log("Activating mitigation");
            
            combatLog.PlayerAppliedBuff(move);

            switch (move.GetEffectType())
            {
                case CombatEffectType.PhysMitigation: 
                    float physMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = physMitigation / 100;
                    unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.PhysMitigation);
                    break;
                case CombatEffectType.MagicMitigation: 
                    float magicMitigation = move.GetPower();
                    unit.CurrentMagicalMitigation = magicMitigation / 100;
                    unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.MagicMitigation);
                    break;
                case CombatEffectType.AllMitigation:
                    float allPhysMitigation = move.GetPower();
                    float allMagicMitigation = move.GetPower();
                    unit.CurrentPhysicalMitigation = allPhysMitigation / 100;
                    unit.CurrentMagicalMitigation = allMagicMitigation / 100;
                    unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.PhysMitigation);
                    unit.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.MagicMitigation);
                    break;
            }
        }
    }
    
    private TakeDamageResult ExecuteSufferTypeMove(CombatUnit attacker, CombatUnit target, CombatMove move)
    {
        
        if (move.GetEffectType().Equals(CombatEffectType.Poison))
        {
            Debug.Log("Added Poison effect");
            target.GetComponent<CombatEffectManager>().AddCombatEffect(move, CombatEffectType.Poison);
            
            combatLog.PlayerAppliedDebuff(move, target);
            return new TakeDamageResult(false, 0);
        }
        else
        {
            combatLog.UsedOffensiveCombatMove(move, attacker, target, move.GetPower());
            return target.TakeDamage(move.GetPower(), CombatMoveType.Suffer);
        }
    }
}
