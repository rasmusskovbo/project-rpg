using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Consumable Item")]
public class ConsumableItem : InventoryItem
{
    public ConsumableItem()
    {
        itemType = ItemType.Consumable;
    }

    public override InventoryItem UseItem(EquipmentManager equipmentManager)
    {
        // Use item funct here.
        throw new System.NotImplementedException();
    }
}
