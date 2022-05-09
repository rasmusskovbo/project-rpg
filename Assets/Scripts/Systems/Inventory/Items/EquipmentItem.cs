using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Equipment Item")]
public class EquipmentItem : InventoryItem
{
    [SerializeField] private EquipmentType equipmentType;
    
    public EquipmentItem()
    {
        itemType = ItemType.Equipment;
    }

    public override void UseItem(EquipmentController equipmentController)
    {
        equipmentController.AssignEquipmentItem(this);
    }

    public EquipmentType EquipmentType
    {
        get => equipmentType;
        set => equipmentType = value;
    }
}