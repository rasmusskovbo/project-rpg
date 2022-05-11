using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Consumable Item")]
[Serializable]
public class ConsumableItem : InventoryItem
{
    public ConsumableItem()
    {
        itemType = ItemType.Consumable;
    }

    public override void UseItem(EquipmentManager equipmentManager)
    {
        // Use item funct here.
        throw new System.NotImplementedException();
    }
}
