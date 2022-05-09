using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private UIInventoryController inventoryUI;

    private EquipmentItem currentHeadItem;
    private EquipmentItem currentChestItem;
    private EquipmentItem currentWaistItem;
    private EquipmentItem currentFeetItem;
    private EquipmentItem currentNeckItem;
    private EquipmentItem currentWeaponItem;
    private EquipmentItem currentShieldItem;
    
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void AssignEquipmentItem(EquipmentItem item)
    {
        switch (item.EquipmentType)
        {
            case EquipmentType.Chest:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Chest ));
                
                // Todo logic for swapping equipment
                if (currentChestItem != null)
                {
                        
                }
                
                break;
        }
    }
}
