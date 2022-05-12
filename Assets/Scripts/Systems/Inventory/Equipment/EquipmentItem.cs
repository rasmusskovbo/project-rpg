using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Equipment Item")]
[Serializable]
public class EquipmentItem : InventoryItem
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private List<StatBonus> statBonuses;
    
    public EquipmentItem()
    {
        itemType = ItemType.Equipment;
    }

    public override void UseItem()
    {
        FindObjectOfType<EquipmentManager>().AssignEquipmentItem(this);
    }

    public EquipmentType EquipmentType
    {
        get => equipmentType;
        set => equipmentType = value;
    }

    public List<StatBonus> GetAllStatBonuses()
    {
        return statBonuses;
    }
    
}