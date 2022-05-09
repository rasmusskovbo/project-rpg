﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItemWrapper> items = new List<InventoryItemWrapper>();
    [SerializeField] private UIInventoryController inventoryUI;

    private Dictionary<InventoryItem, int> itemCountMap = new Dictionary<InventoryItem, int>();

    public void InitInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemCountMap.Add(items[i].Item, items[i].Count);
        }
    }

    public void OpenInventoryUI()
    {
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.InitInventoryUI(this);
    }

    public void UseItem(InventoryItem item)
    {
        // If equippable switch with equipped item in slot
        // If consumable, remove item from inventory, destroy it and apply effects
        Debug.Log(string.Format("Used item: {0}", item.ItemName));
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

        inventoryUI.CreateOrUpdateSlot(this, item, amountToAdd);
    }
    
    public void RemoveItem(InventoryItem item, int amountToRemove)
    {
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
}
