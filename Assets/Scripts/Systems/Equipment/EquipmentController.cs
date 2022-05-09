
using System;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    private InventoryManager _inventoryController;
    private UIInventoryController inventoryUI;

    private void Start()
    {
        _inventoryController = FindObjectOfType<InventoryManager>();
    }
}
