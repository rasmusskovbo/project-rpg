using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Consumable Item")]
public class ConsumableItem : InventoryItem
{
    public ConsumableItem()
    {
        itemType = ItemType.Consumable;
    }

    public override void UseItem(EquipmentController equipmentController)
    {
        // Use item funct here.
        throw new System.NotImplementedException();
    }
}
