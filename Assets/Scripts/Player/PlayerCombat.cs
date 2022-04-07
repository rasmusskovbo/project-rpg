using UnityEngine;

public class PlayerCombat : Unit
{
    // Growth multipliers
    private float maxHpGrowth = 10;
    private float attackPowerGrowth = 5;
    private float abilityPowerGrowth = 5;
    private float physicalDefenseGrowth = 5;
    private float magicalDefenseGrowth = 5;
    private float dodgeGrowth = 5;
    private float speedGrowth = 5;
    
    // Combat tracking stats:
    private float currentHp;
    private float currentMaxHp;
    private float currentAttackPower;
    private float currentAbilityPower;
    private float currentPhysDef;
    private float currentMagicDef;
    private float currentSpeed;
    //private CombatStatus _status = CombatStatus.None;
    
}