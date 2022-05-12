using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, IDataPersistence, ItemHandler
{
    private InventoryManager inventoryManager;
    private UIEquipmentController equipmentUi;
    private GameManager gameManager;
    private UnitBase playerStats;

    private EquipmentItem currentHeadItem;
    private EquipmentItem currentChestItem;
    private EquipmentItem currentWaistItem;
    private EquipmentItem currentFeetItem;
    private EquipmentItem currentNeckItem;
    private EquipmentItem currentWeaponItem;
    private EquipmentItem currentShieldItem;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        equipmentUi = FindObjectOfType<UIEquipmentController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void AssignEquipmentItem(EquipmentItem item)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();
        
        switch (item.EquipmentType)
        {
            case EquipmentType.Head:
                EquipItem(ref currentHeadItem, item);
                break;
            case EquipmentType.Chest:
                EquipItem(ref currentChestItem, item);
                break;
            case EquipmentType.Waist:
                EquipItem(ref currentWaistItem, item);
                break;
            case EquipmentType.Feet:
                EquipItem(ref currentFeetItem, item);
                break;
            case EquipmentType.Neck:
                EquipItem(ref currentNeckItem, item);
                break;
            case EquipmentType.Weapon:
                EquipItem(ref currentWeaponItem, item);
                break;
            case EquipmentType.Shield:
                EquipItem(ref currentShieldItem, item);
                break;
        }
        
        Debug.Log("Equipment Type was not found: " + item.EquipmentType);
    }
    
    public void UnassignEquipmentItem(EquipmentType position)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();
        
        switch (position)
        {
            case EquipmentType.Head:
                UnequipItem(ref currentHeadItem);
                break;
            case EquipmentType.Chest:
                UnequipItem(ref currentChestItem);
                break;
            case EquipmentType.Waist:
                UnequipItem(ref currentWaistItem);
                break;
            case EquipmentType.Feet:
                UnequipItem(ref currentFeetItem);
                break;
            case EquipmentType.Neck:
                UnequipItem(ref currentNeckItem);
                break;
            case EquipmentType.Weapon:
                UnequipItem(ref currentWeaponItem);
                break;
            case EquipmentType.Shield:
                UnequipItem(ref currentShieldItem);
                break;
        }
    }

    /*
     * Equips item in slot and returns the unequipped item
     * Returns null if no item was unequipped.
     */
    private void EquipItem(ref EquipmentItem position, EquipmentItem itemToEquip)
    {
        equipmentUi = FindObjectOfType<UIEquipmentController>();
        
        if (position != null)
        {
            EquipmentItem unequippedItem = position;
            position = itemToEquip;
            UpdateStatBonuses(unequippedItem, position);
            equipmentUi.UpdateSelectedSlotOnEquip(itemToEquip); 
            inventoryManager.AddItem(unequippedItem, 1);
        }
        else
        {
            position = itemToEquip;
            UpdateStatBonuses(null, position);
            equipmentUi.UpdateSelectedSlotOnEquip(itemToEquip);
        }
    }

    private void UnequipItem(ref EquipmentItem position)
    {
        UpdateStatBonuses(position, null);
        position = null;
    }
    
    /*
     * Update reference to player's statbase
     * Subtract all stats from unequipped item
     * Add all stats from equipped item to player's stats.
     */
    public void UpdateStatBonuses(EquipmentItem unequippedItem, EquipmentItem equippedItem)
    {
        playerStats = gameManager.PlayerData.unitBase;
        
        if (unequippedItem != null)
        {
            Debug.Log("Trying to remove stat bonuses for: " + unequippedItem.EquipmentType);
            unequippedItem.GetAllStatBonuses().ForEach(statBonus =>
            {
                UpdateStat(statBonus, true);
            });
        }

        if (equippedItem != null)
        {
            equippedItem.GetAllStatBonuses().ForEach(statBonus =>
            {
                UpdateStat(statBonus, false);
            });  
        }

    }

    /*
     * If item is unequipped, subtract the value instead of adding it.
     */
    public void UpdateStat(StatBonus statBonus, bool isUnequipped)
    {
        if (isUnequipped) statBonus.Value *= -1;
        
        switch (statBonus.StatBonusType)
        {
            case StatBonusType.MaxHp:
                playerStats.MaxHp += Mathf.RoundToInt(statBonus.Value);
                break;
            case StatBonusType.Strength:
                playerStats.Strength += statBonus.Value;
                break;
            case StatBonusType.Agility:
                playerStats.Agility += statBonus.Value;
                break;
            case StatBonusType.Intellect:
                playerStats.Intellect += statBonus.Value;
                break;
            case StatBonusType.AttackPower:
                playerStats.AttackPower += statBonus.Value;
                break;
            case StatBonusType.AbilityPower:
                playerStats.AbilityPower += statBonus.Value;
                break;
            case StatBonusType.PhysCritChance:
                playerStats.Agility += statBonus.Value / 100; // E.g. 1% = + 0.01;
                break;
            case StatBonusType.MagicCritChance:
                playerStats.Agility += statBonus.Value / 100;
                break;
            case StatBonusType.Armor:
                playerStats.PhysicalDefense += statBonus.Value;
                break;
            case StatBonusType.MagicArmor:
                playerStats.MagicalDefense += statBonus.Value;
                break;
            case StatBonusType.Block:
                playerStats.PhysicalBlockPower += statBonus.Value;
                break;
            case StatBonusType.Dodge:
                playerStats.DodgeChance += statBonus.Value / 100;
                break;
            case StatBonusType.Speed:
                playerStats.Speed += statBonus.Value;
                break;
        }
        
        if (isUnequipped) statBonus.Value *= -1;
    }

    public List<EquipmentItem> GetAllEquippedItems()
    {
        return new List<EquipmentItem>
        {
            currentHeadItem,
            currentChestItem,
            currentWaistItem,
            currentFeetItem,
            currentNeckItem,
            currentWeaponItem,
            currentShieldItem
        };
    }

    public void LoadData(GameData data)
    {
        currentHeadItem = data.EquipmentData.CurrentHeadItem;
        currentChestItem = data.EquipmentData.CurrentChestItem;
        currentWaistItem = data.EquipmentData.CurrentWaistItem;
        currentFeetItem = data.EquipmentData.CurrentFeetItem;
        currentNeckItem = data.EquipmentData.CurrentNeckItem;
        currentWeaponItem = data.EquipmentData.CurrentWeaponItem;
        currentShieldItem = data.EquipmentData.CurrentShieldItem;
    }

    public void SaveData(GameData data)
    {
        data.EquipmentData.CurrentHeadItem = currentHeadItem;
        data.EquipmentData.CurrentChestItem = currentChestItem;
        data.EquipmentData.CurrentWaistItem = currentWaistItem;
        data.EquipmentData.CurrentFeetItem = currentFeetItem;
        data.EquipmentData.CurrentNeckItem = currentNeckItem;
        data.EquipmentData.CurrentWeaponItem = currentWeaponItem;
        data.EquipmentData.CurrentShieldItem = currentShieldItem;
    }
}
