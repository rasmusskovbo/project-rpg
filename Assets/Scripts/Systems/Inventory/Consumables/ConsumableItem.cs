using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Items/Create Consumable Item")]
[Serializable]
public class ConsumableItem : InventoryItem
{
    [SerializeField] private ConsumableType type;
    [SerializeField] private int value;
    
    public ConsumableItem()
    {
        itemType = ItemType.Consumable;
    }

    public override void UseItem()
    {
        FindObjectOfType<ConsumableManager>().UseItem(this);
    }

    public ConsumableType Type
    {
        get => type;
        set => type = value;
    }

    public int Value
    {
        get => value;
        set => this.value = value;
    }

}
