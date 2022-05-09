using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private string itemName;

    public GameObject InventoryItemPrefab
    {
        get => inventoryItemPrefab;
        set => inventoryItemPrefab = value;
    }

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
}
