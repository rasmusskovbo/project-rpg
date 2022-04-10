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

    public void InitiatePlayerCombatStats()
    {
        InitiateStaticStats();
        InitiateCurrentStats();
    }

    private void InitiateStaticStats()
    {
        // Should load from sessionmanager/playerstatmanager
        UnitName = "Player";
        UnitType = UnitType.PLAYER;
        MaxHp = 30;
        Level = 1;
        AttackPower = 15;
        AbilityPower = 12;
        PhysicalDefense = 10;
        MagicalDefense = 9;
        PhysicalBlockPower = 2;
        Dodge = 5;
        Speed = 15;
    }
}