using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [SerializeField] protected Sprite itemSprite;
    [SerializeField] protected string itemName;
    protected ItemType itemType;

    public abstract InventoryItem UseItem(EquipmentController equipmentController);
    
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

    public ItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }
}
