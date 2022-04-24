using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatUnit : MonoBehaviour, IComparable
{
    [Header("Description")]
    [SerializeField] private string unitName;
    [TextArea] [SerializeField] private string description;
    [SerializeField] private UnitType unitType;
    [SerializeField] private bool isPlayerUnit;

    [Header("VFX")] 
    [SerializeField] private Sprite idleSprite;
    private Slider hpBar;

    [Header("Base Stats")]
    [SerializeField] private int level;
    [SerializeField] private int maxHp;
    [SerializeField] private float strength;
    [SerializeField] private float agility;
    [SerializeField] private float intellect;
    [SerializeField] private float attackPower;
    [SerializeField] private float abilityPower;
    [SerializeField] private float physicalCritChance;
    [SerializeField] private float magicalCritChance;
    [SerializeField] private float physicalDefense;
    [SerializeField] private float magicalDefense;
    [SerializeField] private float physicalBlockPower;
    [SerializeField] private float dodgeChance;
    [SerializeField] private float speed;

    [Header("Stat Ratios")] 
    [SerializeField] private float strengthApRatio = 2;
    [SerializeField] private float agilityApRatio = 1;
    [SerializeField] private float intellectAbpRatio = 2;
    [SerializeField] private float agilityCritRatio = 0.05f;
    [SerializeField] private float intellectCritRatio = 0.05f;
    [SerializeField] private float strengthPhysDefRatio = 1;
    [SerializeField] private float agilityPhysDefRatio = 2;
    [SerializeField] private float agilityDodgeRatio = 0.05f;
    [SerializeField] private float agilitySpeedRatio = 1;
    [SerializeField] private float critMultiplier = 2;
    
    [Header("Growth Rates")]
    [SerializeField] private float maxHpGrowth = 2;
    [SerializeField] private float strengthGrowth = 1;
    [SerializeField] private float agilityGrowth = 1;
    [SerializeField] private float intellectGrowth = 1;
    [SerializeField] private float physicalDefenseGrowth = 1;
    [SerializeField] private float magicalDefenseGrowth = 2;
    [SerializeField] private float physicalBlockPowerGrowth = 1;
    [SerializeField] private float speedGrowth = 2;
    
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

    public void InitiateCurrentStatsForCombat(int spawnLevel)
    {
        Level = spawnLevel;
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
                
                finalDamage = critCounter < (CurrentPhysicalCritChance * 100) ? damageAfterAttackPower * critMultiplier : damageAfterAttackPower;
                break;
            case CombatMoveType.Magical:
                float damageAfterAbilityPower = power + CurrentAbilityPower;
                
                finalDamage = critCounter < (CurrentMagicalCritChance * 100) ? damageAfterAbilityPower * critMultiplier : damageAfterAbilityPower;
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
        
        if ((currentHp + amount) > maxHp)
        {
            Debug.Log("Healed "+ UnitName +" to maxHP ("+maxHp+")");
            currentHp = maxHp;
        }
        else
        {
            Debug.Log("Healed "+ UnitName +" for " + amount);
            currentHp += amount;
            currentHp = Mathf.Round(currentHp);
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
        get => Mathf.RoundToInt(maxHp + (maxHpGrowth * (level - 1)));
        set => maxHp = value;
    }

    public float Strength
    {
        get => strength + (strengthGrowth * (level - 1));
        set => strength = value;
    }

    public float Agility
    {
        get => agility + (agilityGrowth * (level - 1));
        set => agility = value;
    }

    public float Intellect
    {
        get => intellect + (intellectGrowth * (level - 1));
        set => intellect = value;
    }

    // Combined stats
    public float AttackPower
    {
        get => attackPower
                + (strength * strengthApRatio)
                + (agility * agilityApRatio);
        
        set => attackPower = value;
    }

    public float AbilityPower
    {
        get => abilityPower 
               + (intellect * intellectAbpRatio);
        set => abilityPower = value;
    }

    public float PhysicalCritChance
    {
        get => physicalCritChance 
               + (agility * agilityCritRatio) / 100;
        set => physicalCritChance = value;
    }

    public float MagicalCritChance
    {
        get => magicalCritChance 
               + (intellect * intellectCritRatio) / 100;
        set => magicalCritChance = value;
    }

    public float PhysicalDefense
    {
        get => 
            physicalDefense 
               + (physicalDefenseGrowth * (level - 1))
               + (strength * strengthPhysDefRatio)
               + (agility * agilityPhysDefRatio);
        set => physicalDefense = value;
    }

    public float MagicalDefense
    {
        get => magicalDefense 
               + (magicalDefenseGrowth * (level - 1));
        set => magicalDefense = value;
    }

    public float PhysicalBlockPower
    {
        get => physicalBlockPower 
               + (physicalBlockPowerGrowth * (level - 1));
        set => physicalBlockPower = value;
    }

    public float DodgeChance
    {
        get => dodgeChance 
               + (agility * agilityDodgeRatio) / 100;
        set => dodgeChance = value;
    }

    public float Speed
    {
        get => speed 
               + (speedGrowth * (level - 1))
               + (agility * agilitySpeedRatio);
        set => speed = value;
    }
    
    /// Description
    public string UnitName
    {
        get => unitName;
        set => unitName = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public UnitType UnitType
    {
        get => unitType;
        set => unitType = value;
    }

    public bool IsPlayerUnit
    {
        get => isPlayerUnit;
        set => isPlayerUnit = value;
    }

    public Sprite IdleSprite
    {
        get => idleSprite;
        set => idleSprite = value;
    }

    
    // Ratios
    public float StrengthApRatio
    {
        get => strengthApRatio;
        set => strengthApRatio = value;
    }

    public float AgilityApRatio
    {
        get => agilityApRatio;
        set => agilityApRatio = value;
    }

    public float IntellectAbpRatio
    {
        get => intellectAbpRatio;
        set => intellectAbpRatio = value;
    }

    public float AgilityCritRatio
    {
        get => agilityCritRatio;
        set => agilityCritRatio = value;
    }

    public float IntellectCritRatio
    {
        get => intellectCritRatio;
        set => intellectCritRatio = value;
    }

    public float StrengthPhysDefRatio
    {
        get => strengthPhysDefRatio;
        set => strengthPhysDefRatio = value;
    }

    public float AgilityPhysDefRatio
    {
        get => agilityPhysDefRatio;
        set => agilityPhysDefRatio = value;
    }

    public float AgilityDodgeRatio
    {
        get => agilityDodgeRatio;
        set => agilityDodgeRatio = value;
    }

    public float AgilitySpeedRatio
    {
        get => agilitySpeedRatio;
        set => agilitySpeedRatio = value;
    }

    // Growth
    public float MaxHpGrowth
    {
        get => maxHpGrowth;
        set => maxHpGrowth = value;
    }

    public float StrengthGrowth
    {
        get => strengthGrowth;
        set => strengthGrowth = value;
    }

    public float AgilityGrowth
    {
        get => agilityGrowth;
        set => agilityGrowth = value;
    }

    public float IntellectGrowth
    {
        get => intellectGrowth;
        set => intellectGrowth = value;
    }

    public float PhysicalDefenseGrowth
    {
        get => physicalDefenseGrowth;
        set => physicalDefenseGrowth = value;
    }

    public float MagicalDefenseGrowth
    {
        get => magicalDefenseGrowth;
        set => magicalDefenseGrowth = value;
    }

    public float PhysicalBlockPowerGrowth
    {
        get => physicalBlockPowerGrowth;
        set => physicalBlockPowerGrowth = value;
    }

    public float SpeedGrowth
    {
        get => speedGrowth;
        set => speedGrowth = value;
    }

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
        get => currentAttackPower * combatEffectsManager.GetEffectMultiplier(CombatEffectType.Strengthen) * (1 - combatEffectsManager.GetEffectMultiplier(CombatEffectType.Weaken));
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
