using System;
using UnityEngine;

[Serializable]
public abstract class InventoryItem : ScriptableObject
{
    [SerializeField] protected Sprite itemSprite;
    [SerializeField] protected string itemName;
    [SerializeField] protected TooltipInfo tooltipInfo;
    protected ItemType itemType;

    public abstract void UseItem();
    
    public Sprite ItemSprite
    {
        get => itemSprite;
        set => itemSprite = value;
    }

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public TooltipInfo TooltipInfo
    {
        get => tooltipInfo;
        set => tooltipInfo = value;
    }

    public ItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }
}
