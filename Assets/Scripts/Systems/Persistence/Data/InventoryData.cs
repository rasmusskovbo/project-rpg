using System;
using UnityEngine;

[Serializable]
public class InventoryData : SaveData
{
    [SerializeField] private SerializableDictionary<InventoryItem, int> inventoryMap;

    public InventoryData(SerializableDictionary<InventoryItem, int> inventoryMap)
    {
        this.inventoryMap = inventoryMap;
    }

    public SerializableDictionary<InventoryItem, int> InventoryMap
    {
        get => inventoryMap;
        set => inventoryMap = value;
    }

    public void ResetBeforeSave()
    {
        inventoryMap = new SerializableDictionary<InventoryItem, int>();
    }
}
