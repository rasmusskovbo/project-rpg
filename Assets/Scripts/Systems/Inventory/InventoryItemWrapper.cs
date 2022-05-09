using System;
using UnityEngine;

[Serializable]
public class InventoryItemWrapper
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private int count;

    public InventoryItem Item
    {
        get => item;
        set => item = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }
}
