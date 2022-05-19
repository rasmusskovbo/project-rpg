using System;
using UnityEngine;

[Serializable]
public class EquipmentData : SaveData
{
    [SerializeField] private EquipmentItem currentHeadItem;
    [SerializeField] private EquipmentItem currentChestItem;
    [SerializeField] private EquipmentItem currentWaistItem;
    [SerializeField] private EquipmentItem currentFeetItem;
    [SerializeField] private EquipmentItem currentNeckItem;
    [SerializeField] private EquipmentItem currentWeaponItem;
    [SerializeField] private EquipmentItem currentShieldItem;

    public EquipmentData(EquipmentItem currentHeadItem, EquipmentItem currentChestItem, EquipmentItem currentWaistItem, EquipmentItem currentFeetItem, EquipmentItem currentNeckItem, EquipmentItem currentWeaponItem, EquipmentItem currentShieldItem)
    {
        this.currentHeadItem = currentHeadItem;
        this.currentChestItem = currentChestItem;
        this.currentWaistItem = currentWaistItem;
        this.currentFeetItem = currentFeetItem;
        this.currentNeckItem = currentNeckItem;
        this.currentWeaponItem = currentWeaponItem;
        this.currentShieldItem = currentShieldItem;
    }

    public EquipmentItem CurrentHeadItem
    {
        get => currentHeadItem;
        set => currentHeadItem = value;
    }

    public EquipmentItem CurrentChestItem
    {
        get => currentChestItem;
        set => currentChestItem = value;
    }

    public EquipmentItem CurrentWaistItem
    {
        get => currentWaistItem;
        set => currentWaistItem = value;
    }

    public EquipmentItem CurrentFeetItem
    {
        get => currentFeetItem;
        set => currentFeetItem = value;
    }

    public EquipmentItem CurrentNeckItem
    {
        get => currentNeckItem;
        set => currentNeckItem = value;
    }

    public EquipmentItem CurrentWeaponItem
    {
        get => currentWeaponItem;
        set => currentWeaponItem = value;
    }

    public EquipmentItem CurrentShieldItem
    {
        get => currentShieldItem;
        set => currentShieldItem = value;
    }
    
    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}
