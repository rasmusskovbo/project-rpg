using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    [Header("Blocking")]
    [SerializeField] private bool consumeBlock;
    [SerializeField] private bool expireBlock;
    [SerializeField] private bool blockingCanBeSplit;
    private BlockTalents blockTalents;

    [Header("Mitigation")] 
    [SerializeField] private bool consumeMitigation;

    private CombatUnit unit;

    private void Awake()
    {
        unit = GetComponent<CombatUnit>();
    }

    public float CalculateBlock(float damageAfterMitigation)
    {
        Debug.Log("Calculating Block");
        float totalPhysicalBlock = unit.CurrentPhysicalBlock + unit.PhysicalBlockPower;
        Debug.Log(totalPhysicalBlock);
        float damageAfterBlock = Mathf.Clamp((damageAfterMitigation - totalPhysicalBlock), 0,
            float.MaxValue);

        // Handle/reset block power
        // Possible refactor to talent tree.
        if (ConsumeBlock)
        {
            if (damageAfterMitigation > totalPhysicalBlock)
            {
                // Consume all current phys block and subtract active block from damage
                unit.CurrentPhysicalBlock = 0;
            }
            else // if damage is less than our current block
            {
                // if we can save leftover block
                if (BlockingCanBeSplit)
                {
                    // only subtract whats used
                    unit.CurrentPhysicalBlock -= damageAfterMitigation;
                }
                else
                {
                    unit.CurrentPhysicalBlock = 0;
                }
            }
        }

        return damageAfterBlock;

    }

    public float CalculatePhysicalMitigation(float damage)
    {
        float damageAfterMitigation = damage * (1 - unit.CurrentPhysicalMitigation);
    
        if (consumeMitigation)
        {
            unit.CurrentPhysicalMitigation = 0;
        }

        return damageAfterMitigation;
    }
    
    public float CalculateArmor(float damageAfterBlock)
    {
        return Mathf.Clamp((damageAfterBlock - unit.CurrentPhysDef), 0, float.MaxValue);
    }

        
    
        
        
        
    public bool ConsumeBlock
    {
        get => consumeBlock;
        set => consumeBlock = value;
    }

    public bool ExpireBlock
    {
        get => expireBlock;
        set => expireBlock = value;
    }

    public bool BlockingCanBeSplit
    {
        get => blockingCanBeSplit;
        set => blockingCanBeSplit = value;
    }
}
