using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private UIInventoryController inventoryUI;
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
        inventoryManager = FindObjectOfType<InventoryManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public EquipmentItem AssignEquipmentItem(EquipmentItem item)
    {
        switch (item.EquipmentType)
        {
            case EquipmentType.Head:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Head ));
                return EquipItem(ref currentHeadItem, item);
            case EquipmentType.Chest:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Chest ));
                return EquipItem(ref currentChestItem, item);
            case EquipmentType.Waist:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Waist ));
                return EquipItem(ref currentWaistItem, item);
            case EquipmentType.Feet:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Feet ));
                return EquipItem(ref currentFeetItem, item);
            case EquipmentType.Neck:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Neck ));
                return EquipItem(ref currentNeckItem, item);
            case EquipmentType.Weapon:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Weapon ));
                return EquipItem(ref currentWeaponItem, item);
            case EquipmentType.Shield:
                Debug.Log(string.Format("Equipped {0} to :{1}", item.ItemName, EquipmentType.Shield ));
                return EquipItem(ref currentShieldItem, item);
        }

        Debug.Log("Equipment Type was not found: " + item.EquipmentType);
        return null;
    }

    /*
     * Equips item in slot and returns the unequipped item
     * Returns null if no item was unequipped.
     */
    private EquipmentItem EquipItem(ref EquipmentItem position, EquipmentItem itemToEquip)
    {
        if (position != null)
        {
            EquipmentItem unequippedItem = position;
            position = itemToEquip;
            UpdateStatBonuses(unequippedItem, position);
            return unequippedItem;
        }
        else
        {
            position = itemToEquip;
            UpdateStatBonuses(null, position);
            return null;
        }
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
        
        equippedItem.GetAllStatBonuses().ForEach(statBonus =>
        {
            UpdateStat(statBonus, false);
        });
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
}
