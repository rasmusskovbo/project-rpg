using System;
using UnityEngine;

[Serializable]
public class InventoryData
{
    [SerializeField] public SerializableDictionary<InventoryItem, int> inventoryMap;

    public InventoryData(SerializableDictionary<InventoryItem, int> inventoryMap)
    {
        this.inventoryMap = inventoryMap;
    }
}
