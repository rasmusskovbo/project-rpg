using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatUnit : MonoBehaviour, IComparable
{
    [Header("Base Stats")]
    private UnitBase unitBase;
    private Sprite idleSprite;
    [SerializeField] private int level;
    
    // Combat tracking stats:
    [Header("Stat Inspection")]
    [SerializeField] private float currentHp;
    [SerializeField] private float currentMaxHp;
    [SerializeField] private float currentStrength;
    [SerializeField] private float currentAgility;
    [SerializeField] private float currentIntellect;
    [SerializeField] private float currentAttackPower;
    [SerializeField] private float currentAbilityPower;
    [SerializeField] private float currentPhysicalCritChance;
    [SerializeField] private float currentMagicalCritChance;
    [SerializeField] private float currentPhysDef;
    [SerializeField] private float currentMagicDef;
    [SerializeField] private float currentPhysicalMitigation;
    [SerializeField] private float currentPhysicalBlock;
    [SerializeField] private float currentMagicalMitigation;
    [SerializeField] private float currentDodge;
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isAlive;
    
    private List<CombatMoveType> acceptedDamageTypes = new List<CombatMoveType>
        {CombatMoveType.Physical, CombatMoveType.Magical, CombatMoveType.Suffer};
    
    private TalentManager talentManager;
    private CombatEffectManager combatEffectsManager;

    private void Awake()
    {
        talentManager = GetComponent<TalentManager>();
        combatEffectsManager = GetComponent<CombatEffectManager>();
    }

    public void InitiateUnit(UnitBase unitbase, int spawnLevel)
    {
        Level = spawnLevel;
        this.unitBase = unitbase;
        
        CurrentHp = MaxHp;
        CurrentMaxHp = MaxHp;
        CurrentStrength = Strength;
        CurrentAgility = Agility;
        CurrentIntellect = Intellect;
        CurrentAttackPower = AttackPower;
        CurrentAbilityPower = AbilityPower;
        CurrentPhysicalCritChance = PhysicalCritChance;
        CurrentMagicalCritChance = MagicalCritChance;
        CurrentPhysDef = PhysicalDefense;
        CurrentMagicDef = MagicalDefense;
        CurrentPhysicalMitigation = 0;
        CurrentPhysicalBlock = 0;
        CurrentMagicalMitigation = 0;
        CurrentDodge = DodgeChance;
        CurrentSpeed = Speed;
    }

    public float DoDamage(CombatMove move)
    {
        if (!acceptedDamageTypes.Contains(move.GetType()))
        {
            Debug.Log("Attempted to damage with wrong type: " + move.GetType());
            return 0;
        }

        float power = move.GetPower();
        float finalDamage = power;
        float critCounter = Random.Range(0, 100);

        Debug.Log("Chance it's a crit: " + critCounter);
        
        switch (move.GetType())
        {
            case CombatMoveType.Physical:
                float damageAfterAttackPower = power + CurrentAttackPower;

                finalDamage = critCounter < (CurrentPhysicalCritChance * 100) ? damageAfterAttackPower * unitBase.CritMultiplier : damageAfterAttackPower;
                break;
            case CombatMoveType.Magical:
                float damageAfterAbilityPower = power + CurrentAbilityPower;
                
                finalDamage = critCounter < (CurrentMagicalCritChance * 100) ? damageAfterAbilityPower * unitBase.CritMultiplier : damageAfterAbilityPower;
                break;
        }

        return finalDamage;
    }

    public TakeDamageResult TakeDamage(float damage, CombatMoveType moveType)
    {
        if (!acceptedDamageTypes.Contains(moveType))
        {
            Debug.Log("Attempted to damage with wrong type: " + moveType);
            return null;
        }

        switch (moveType)
        {
            case CombatMoveType.Physical:
            { 
            /*
             * Check if attack is dodged.
             * Mitigate total damage of skill with percentage (e.g. 10%, 25%) and round -- // 11 * (1 - 0.1) = 9.9 -> 10;
             * Block damage of skill by a flat amount, decided by currentPhysicalBlock (if any was applied) times the block power of unit. -- // 10 - (2 * 1) = 8;
             * Subtract physical armor from damage of skill -- // 5 - 2 = 3;
             * Take damage
             * Mathf.Round to round to nearest whole number
             * Mathf.Clamp to avoid subtracting negative numbers
             * Damage is returned rounded to int for UI purposes
             */
                float hitChance = Random.Range(0, 100);

                Debug.Log("Hit chance: " + hitChance);
                Debug.Log("Dodge chance: " + CurrentDodge);
                if (hitChance < CurrentDodge * 100)
                {
                    Debug.Log("Attack was dodged");
                    return new TakeDamageResult(this, false, 0);
                }
                else
                {
                    Debug.Log("initial damage: " + damage);
                    
                    // Calculate mitigation
                    // Mitigation should be active effect
                    float damageAfterMitigation = damage;
                    if (combatEffectsManager.IsEffectActive(CombatEffectType.PhysMitigation))
                    {
                        damageAfterMitigation = talentManager.CalculatePhysicalMitigation(damage);
                    }
                    
                    // Calculate block
                    // Only apply if block is active on unit, if not skip.
                    float damageAfterBlock = damageAfterMitigation;
                    if (combatEffectsManager.IsEffectActive(CombatEffectType.Block))
                    {
                        damageAfterBlock = talentManager.CalculateBlock(damageAfterMitigation);
                    }
                    
                    // Calculate armor
                    float damageAfterArmor = talentManager.CalculateArmor(damageAfterBlock);
                    
                    // Round to nearest int.
                    int finalDamageAfterArmor = Mathf.RoundToInt(damageAfterArmor);

                    Debug.Log("Damage taken: " + finalDamageAfterArmor);
                    CurrentHp -= finalDamageAfterArmor;
            
                    return new TakeDamageResult(this, CurrentHp <= 0, finalDamageAfterArmor);;    
                }
            }
            case CombatMoveType.Magical:
            {
                int damageAfterMitigation = Mathf.RoundToInt(damage * (1 - CurrentMagicalMitigation));
                float damageAfterArmor = Mathf.Clamp(
                    (damageAfterMitigation - CurrentMagicDef),
                    0, float.MaxValue);
                int finalDamageAfterArmor = Mathf.RoundToInt(damageAfterArmor);

                CurrentHp -= finalDamageAfterArmor;

                return new TakeDamageResult(this, CurrentHp <= 0, finalDamageAfterArmor);
            }
            // Suffer Damage
            default:
                CurrentHp -= Mathf.RoundToInt(damage);

                return new TakeDamageResult(this, CurrentHp <= 0, Mathf.RoundToInt(damage));
        }
    }
    
    public void Heal(float amount)
    {
        
        if ((CurrentHp + amount) > CurrentMaxHp)
        {
            CurrentHp = CurrentMaxHp;
        }
        else
        {
            CurrentHp += amount;
            CurrentHp = Mathf.Round(CurrentHp);
        }
        
    }
    
    // Compare Units by Speed
    public int CompareTo(object obj)
    {
        CombatUnit other = obj as CombatUnit;
        return other.CurrentSpeed.CompareTo(this.CurrentSpeed);
    }
    
    /// Getters
    // Base stats
    public int Level
    {
        get => level;
        set => level = value;
    }

    public int MaxHp
    {
        get => Mathf.RoundToInt(unitBase.MaxHp + (unitBase.MaxHpGrowth * (level - 1)));
    }

    public float Strength
    {
        get => unitBase.IsPlayerUnit ? unitBase.Strength : unitBase.Strength + (unitBase.StrengthGrowth * (level - 1));
    }

    public float Agility
    {
        get => unitBase.IsPlayerUnit ? unitBase.Agility : unitBase.Agility + (unitBase.AgilityGrowth * (level - 1));
    }

    public float Intellect
    {
        get => unitBase.IsPlayerUnit ? unitBase.Intellect : unitBase.Intellect + (unitBase.Intellect * (level - 1));
    }

    // Combined stats
    public float AttackPower
    {
        get => unitBase.AttackPower
               + (unitBase.Strength * unitBase.StrengthApRatio)
               + (unitBase.Agility * unitBase.AgilityApRatio);

    }

    public float AbilityPower
    {
        get => unitBase.AbilityPower 
               + (unitBase.Intellect * unitBase.IntellectAbpRatio);
    }

    public float PhysicalCritChance
    {
        get => unitBase.PhysicalCritChance 
               + (unitBase.Agility * unitBase.AgilityCritRatio) / 100;
    }

    public float MagicalCritChance
    {
        get => unitBase.MagicalCritChance 
               + (unitBase.Intellect * unitBase.IntellectCritRatio) / 100;
    }

    public float PhysicalDefense
    {
        get => 
            unitBase.PhysicalDefense 
               + (unitBase.PhysicalDefenseGrowth * (level - 1))
               + (unitBase.Strength * unitBase.StrengthPhysDefRatio)
               + (unitBase.Agility * unitBase.AgilityPhysDefRatio);
    }

    public float MagicalDefense
    {
        get => unitBase.MagicalDefense 
               + (unitBase.MagicalDefenseGrowth * (level - 1));
    }

    public float PhysicalBlockPower
    {
        get => unitBase.PhysicalBlockPower 
               + (unitBase.PhysicalBlockPowerGrowth * (level - 1));
    }

    public float DodgeChance
    {
        get => unitBase.DodgeChance 
               + (unitBase.Agility * unitBase.AgilityDodgeRatio) / 100;
    }

    public float Speed
    {
        get => unitBase.Speed 
               + (unitBase.SpeedGrowth * (level - 1))
               + (unitBase.Agility * unitBase.AgilitySpeedRatio);
    }
    
    /// Description
    public string UnitName
    {
        get => unitBase.UnitName;
        set => unitBase.UnitName = value;
    }

    public string Description
    {
        get => unitBase.Description;
        set => unitBase.Description = value;
    }

    public UnitType UnitType
    {
        get => unitBase.UnitType;
        set => unitBase.UnitType = value;
    }

    public bool IsPlayerUnit
    {
        get => unitBase.IsPlayerUnit;
        set => unitBase.IsPlayerUnit = value;
    }

    public Sprite IdleSprite
    {
        get => unitBase.IdleSprite;
        set => unitBase.IdleSprite = value;
    }

    
    // Ratios
    public float StrengthApRatio
    {
        get => unitBase.StrengthApRatio;
        set => unitBase.StrengthApRatio = value;
    }

    public float AgilityApRatio
    {
        get => unitBase.AgilityApRatio;
        set => unitBase.AgilityApRatio = value;
    }

    public float IntellectAbpRatio
    {
        get => unitBase.IntellectAbpRatio;
        set => unitBase.IntellectAbpRatio = value;
    }

    public float AgilityCritRatio
    {
        get => unitBase.AgilityCritRatio;
        set => unitBase.AgilityCritRatio = value;
    }

    public float IntellectCritRatio
    {
        get => unitBase.IntellectCritRatio;
        set => unitBase.IntellectCritRatio = value;
    }

    public float StrengthPhysDefRatio
    {
        get => unitBase.StrengthPhysDefRatio;
        set => unitBase.StrengthPhysDefRatio = value;
    }

    public float AgilityPhysDefRatio
    {
        get => unitBase.AgilityPhysDefRatio;
        set => unitBase.AgilityPhysDefRatio = value;
    }

    public float AgilityDodgeRatio
    {
        get => unitBase.AgilityDodgeRatio;
        set => unitBase.AgilityDodgeRatio = value;
    }

    public float AgilitySpeedRatio
    {
        get => unitBase.AgilitySpeedRatio;
        set => unitBase.AgilitySpeedRatio = value;
    }

    // Growth
    public float MaxHpGrowth
    {
        get => unitBase.MaxHpGrowth;
        set => unitBase.MaxHpGrowth = value;
    }

    public float StrengthGrowth
    {
        get => unitBase.StrengthGrowth;
        set => unitBase.StrengthGrowth = value;
    }

    public float AgilityGrowth
    {
        get => unitBase.AgilityGrowth;
        set => unitBase.AgilityGrowth = value;
    }

    public float IntellectGrowth
    {
        get => unitBase.IntellectGrowth;
        set => unitBase.IntellectGrowth = value;
    }

    public float PhysicalDefenseGrowth
    {
        get => unitBase.PhysicalDefenseGrowth;
        set => unitBase.PhysicalDefenseGrowth = value;
    }

    public float MagicalDefenseGrowth
    {
        get => unitBase.MagicalDefenseGrowth;
        set => unitBase.MagicalDefenseGrowth = value;
    }

    public float PhysicalBlockPowerGrowth
    {
        get => unitBase.PhysicalBlockPowerGrowth;
        set => unitBase.PhysicalBlockPowerGrowth = value;
    }

    public float SpeedGrowth
    {
        get => unitBase.SpeedGrowth;
        set => unitBase.SpeedGrowth = value;
    }

    /*
     * Current stats (combat tracking)
     */
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

    public float CurrentStrength
    {
        get => currentStrength;
        set => currentStrength = value;
    }

    public float CurrentAgility
    {
        get => currentAgility;
        set => currentAgility = value;
    }

    public float CurrentIntellect
    {
        get => currentIntellect;
        set => currentIntellect = value;
    }

    public float CurrentAttackPower
    {
        get => currentAttackPower * combatEffectsManager.GetEffectMultiplier(CombatEffectType.Strengthen) * (combatEffectsManager.GetEffectMultiplier(CombatEffectType.Weaken));
        set => currentAttackPower = value;
    }

    public float CurrentAbilityPower
    {
        get => currentAbilityPower;
        set => currentAbilityPower = value;
    }

    public float CurrentPhysicalCritChance
    {
        get => currentPhysicalCritChance;
        set => currentPhysicalCritChance = value;
    }

    public float CurrentMagicalCritChance
    {
        get => currentMagicalCritChance;
        set => currentMagicalCritChance = value;
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

    public bool IsAlive
    {
        get => CurrentHp > 0;
        set => isAlive = value;
    }

    public CombatEffectManager CombatEffectsManager
    {
        get => combatEffectsManager;
    }
}
