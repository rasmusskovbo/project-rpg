using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private UIInventorySlot itemSlotPrefab;
    private InventoryManager inventoryManager;
    private Dictionary<InventoryItem, UIInventorySlot> itemSlotMap = new Dictionary<InventoryItem, UIInventorySlot>();

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void InitInventoryUI(Dictionary<InventoryItem, int> itemsMap)
    {
        foreach (var kvp in itemsMap)
        {
            CreateOrUpdateSlot(kvp.Key, kvp.Value);
        }
    }
    
    public void CreateOrUpdateSlot(InventoryItem item, int amount)
    {
        if (!itemSlotMap.ContainsKey(item))
        {
            var slot = CreateSlot(item, amount);
            itemSlotMap.Add(item, slot);
        }
        else
        {
            UpdateSlot(item, amount);
        }
    }

    private UIInventorySlot CreateSlot(InventoryItem item, int amount)
    {
        var slot = Instantiate(itemSlotPrefab, itemsContainer);
        slot.InitSlotVisuals(item.ItemSprite, amount);
        slot.AssignSlotButtonCallback(() => inventoryManager.UseItem(item));
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
