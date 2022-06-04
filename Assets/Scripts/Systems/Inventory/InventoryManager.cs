using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : PersistentSingleton<InventoryManager>, IDataPersistence
{
    [SerializeField] private List<InventoryItemWrapper> items = new List<InventoryItemWrapper>();
    [SerializeField] private bool isDebug;
    private EquipmentManager equipmentManager;
    private UIInventoryController inventoryUI;

    // Should re-fetch reference when called as scene changes will not keep it intact (maybe)
    private void Start()
    {
        equipmentManager = FindObjectOfType<EquipmentManager>();
        inventoryUI = FindObjectOfType<UIInventoryController>();
        InitInventory();
    }

    private Dictionary<InventoryItem, int> itemCountMap = new Dictionary<InventoryItem, int>();

    public void InitInventory()
    {
        if (isDebug || itemCountMap.Count == 0) SpawnItems();
        inventoryUI.InitInventoryUI();
    }

    private void SpawnItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemCountMap.Add(items[i].Item, items[i].Count);
        }
    }

    public void UseItem(InventoryItem item)
    {
        // If equippable switch with equipped item in slot
        // If consumable, remove item from inventory, destroy it and apply effects
        Debug.Log(string.Format("Used item: {0}", item.ItemName));
        if (item.ItemType == ItemType.Equipment)
        {
            RemoveItem(item, 1);
            item.UseItem();
        }

        if (item.ItemType == ItemType.Consumable)
        {
            RemoveItem(item, 1);
            item.UseItem();
        }

    }

    public void AddItem(InventoryItem item, int amountToAdd)
    {
        int currentItemCount;

        if (itemCountMap.TryGetValue(item, out currentItemCount))
        {
            itemCountMap[item] = currentItemCount + amountToAdd;
        }
        else
        {
            itemCountMap.Add(item, amountToAdd);
        }

        inventoryUI.CreateOrUpdateSlot(item, amountToAdd);
    }
    
    public void RemoveItem(InventoryItem item, int amountToRemove)
    {
        inventoryUI = FindObjectOfType<UIInventoryController>();
        
        int currentItemCount;

        if (itemCountMap.TryGetValue(item, out currentItemCount))
        {
            itemCountMap[item] = currentItemCount - amountToRemove;
            if (currentItemCount - amountToRemove <= 0)
            {
                inventoryUI.DestroySlot(item);
            }
            else
            {
                inventoryUI.UpdateSlot(item, currentItemCount - amountToRemove);
            }
        }
        else
        {
            Debug.Log(string.Format("Cannot remove item: {0}. Item was not found in inventory", item.ItemName));
        }
    }

    public List<InventoryItemWrapper> Items
    {
        get => items;
        set => items = value;
    }

    public Dictionary<InventoryItem, int> ItemCountMap
    {
        get => itemCountMap;
        set => itemCountMap = value;
    }

    public void LoadData(GameData data)
    {
        var inventoryMap = new Dictionary<InventoryItem, int>();
        
        foreach (var keyValuePair in data.InventoryData.InventoryMap)
        {
            inventoryMap.Add(keyValuePair.Key, keyValuePair.Value);    
        }

        ItemCountMap = inventoryMap;
    }

    public void SaveData(GameData data)
    {
        data.InventoryData.ResetBeforeSave();
        
        foreach (var keyValuePair in ItemCountMap)
        {
            data.InventoryData.InventoryMap.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
