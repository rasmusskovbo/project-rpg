using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private UIInventorySlot itemSlotPrefab;
    [SerializeField] private Inventory inventory;
    private Dictionary<InventoryItem, UIInventorySlot> itemSlotMap = new Dictionary<InventoryItem, UIInventorySlot>();

    private void Start()
    {
        // Get all items from inventory.
        InitInventoryUI(inventory);
    }

    public void InitInventoryUI(Inventory inventory)
    {
        var itemsMap = inventory.ItemCountMap;
        foreach (var kvp in itemsMap)
        {
            CreateOrUpdateSlot(inventory, kvp.Key, kvp.Value);
        }
    }
    
    public void CreateOrUpdateSlot(Inventory inventory, InventoryItem item, int amount)
    {
        if (!itemSlotMap.ContainsKey(item))
        {
            var slot = CreateSlot(inventory, item, amount);
            itemSlotMap.Add(item, slot);
        }
        else
        {
            UpdateSlot(item, amount);
        }
    }

    private UIInventorySlot CreateSlot(Inventory inventory, InventoryItem item, int amount)
    {
        var slot = Instantiate(itemSlotPrefab, itemsContainer);
        slot.InitSlotVisuals(item.ItemSprite, amount);
        slot.AssignSlotButtonCallback(() => inventory.UseItem(item));
        return slot;
    }

    public void UpdateSlot(InventoryItem item, int itemCount)
    {
        itemSlotMap[item].UpdateSlotCount(itemCount);
    }

    public void DestroySlot(InventoryItem item)
    {
        Destroy(itemSlotMap[item].gameObject);
        itemSlotMap.Remove(item);
    }
}
