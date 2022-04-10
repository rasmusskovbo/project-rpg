using System;
using UnityEngine;

public class Unit : MonoBehaviour, IComparable
{
    // Static stats (loaded upon combat start)
    private string unitName;
    private int maxHp;
    private int level;
    private float attackPower;
    private float abilityPower;
    private float physicalDefense;
    private float magicalDefense;
    private float physicalBlockPower;
    private float dodge;
    private float speed;
    private UnitType unitType;

    // Combat tracking stats:
    private float currentHp;
    private float currentMaxHp;
    private float currentAttackPower;
    private float currentAbilityPower;
    private float currentPhysDef;
    private float currentMagicDef;
    private float currentPhysicalMitigation;
    private float currentPhysicalBlock;
    private float currentMagicalMitigation;
    private float currentDodge;
    private float currentSpeed;
    private CombatStatus _status = CombatStatus.None;

    public void InitiateCurrentStats()
    {
        currentHp = maxHp;
        currentMaxHp = maxHp;
        currentAttackPower = attackPower;
        currentAbilityPower = abilityPower;
        currentPhysDef = physicalDefense;
        currentMagicDef = magicalDefense;
        currentDodge = dodge;
        currentSpeed = speed;
    }
    
    public TakeDamageResult TakePhysicalDamage(float damage)
    {
        // TODO Add dodge
        /*
         * 1) Multiply power of skill with current attack power. This is done in attack script-- // 2 * 5.5 = 11
         * 2a) Mitigate total damage of skill with percentage (e.g. 10%, 25%) and round -- // 11 * (1 - 0.1) = 9.9 -> 10;
         * 2b) Block damage of skill by a flat amount, decided by currentPhysicalBlock (if any was applied) times the block power of unit. -- // 10 - (2 * 1) = 8;
         * 3) Subtract physical armor from damage of skill -- // 5 - 2 = 3;
         * 4) Take damage
         * Mathf.Round to round to nearest whole number
         * Mathf.Clamp to avoid subtracting negative numbers
         * Damage is returned rounded to int for UI purposes
         */
        
        float damageAfterMitigation = damage * (1 - currentPhysicalMitigation);
        float damageAfterBlock = Mathf.Clamp(
            (damageAfterMitigation - (currentPhysicalBlock * physicalBlockPower)),
            0, float.MaxValue);
        float damageAfterArmor = Mathf.Clamp(
            (damageAfterBlock - currentPhysDef),
            0, float.MaxValue);
        int finalDamageAfterArmor = Mathf.RoundToInt(damageAfterArmor);
        
        currentHp -= finalDamageAfterArmor;

        TakeDamageResult result = new TakeDamageResult(currentHp <= 0, finalDamageAfterArmor);
        
        return result;
    }
    
    // Static stats
    public string UnitName
    {
        get => unitName;
        set => unitName = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    public float AbilityPower
    {
        get => abilityPower;
        set => abilityPower = value;
    }

    public float PhysicalDefense
    {
        get => physicalDefense;
        set => physicalDefense = value;
    }

    public float MagicalDefense
    {
        get => magicalDefense;
        set => magicalDefense = value;
    }

    public float PhysicalBlockPower
    {
        get => physicalBlockPower;
        set => physicalBlockPower = value;
    }

    public float Dodge
    {
        get => dodge;
        set => dodge = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public UnitType UnitType
    {
        get => unitType;
        set => unitType = value;
    }
    
    // Current Stats
    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = value;
    }

    public float CurrentMaxHp
    {
        get => currentMaxHp;
        set => currentMaxHp = value;
    }

    public float CurrentAttackPower
    {
        get => currentAttackPower;
        set => currentAttackPower = value;
    }

    public float CurrentAbilityPower
    {
        get => currentAbilityPower;
        set => currentAbilityPower = value;
    }

    public float CurrentPhysDef
    {
        get => currentPhysDef;
        set => currentPhysDef = value;
    }

    public float CurrentMagicDef
    {
        get => currentMagicDef;
        set => currentMagicDef = value;
    }

    public float CurrentPhysicalMitigation
    {
        get => currentPhysicalMitigation;
        set => currentPhysicalMitigation = value;
    }

    public float CurrentPhysicalBlock
    {
        get => currentPhysicalBlock;
        set => currentPhysicalBlock = value;
    }

    public float CurrentMagicalMitigation
    {
        get => currentMagicalMitigation;
        set => currentMagicalMitigation = value;
    }

    public float CurrentDodge
    {
        get => currentDodge;
        set => currentDodge = value;
    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    public CombatStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public int CompareTo(object obj)
    {
        Unit other = obj as Unit;
        return other.CurrentSpeed.CompareTo(this.CurrentSpeed);
        //return Mathf.RoundToInt(this.CurrentSpeed - other.CurrentSpeed);
    }


}
