using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO MissingReferenceException: Problemer med at equippe items efter scene skift.
// Tjek referencer til equipment manager eller evt lav denne klasse om til persistent.
public class UIEquipmentController : MonoBehaviour
{
    [SerializeField] private UIEquipmentSlot headSlot;
    [SerializeField] private UIEquipmentSlot chestSlot;
    [SerializeField] private UIEquipmentSlot waistSlot;
    [SerializeField] private UIEquipmentSlot feetSlot;
    [SerializeField] private UIEquipmentSlot neckSlot;
    [SerializeField] private UIEquipmentSlot weaponSlot;
    [SerializeField] private UIEquipmentSlot shieldSlot;

    private EquipmentManager equipmentManager;
    private List<UIEquipmentSlot> allSlots;

    private void Start()
    {
        equipmentManager = FindObjectOfType<EquipmentManager>();
        
        allSlots = new List<UIEquipmentSlot>
        {
            headSlot,
            chestSlot,
            waistSlot,
            feetSlot,
            neckSlot,
            weaponSlot,
            shieldSlot
        };

        allSlots.ForEach(uiSlot => uiSlot.EquipmentIcon.gameObject.SetActive(false));
        
        equipmentManager.GetAllEquippedItems().ForEach(equippedItem =>
        {
            if (equippedItem != null)
            {
                UpdateSelectedSlotOnEquip(equippedItem);
            }
        });
        
    }

    public void UpdateSelectedSlotOnEquip(EquipmentItem item)
    {
        allSlots.ForEach(uiSlot =>
        {
            if (uiSlot.EquipmentType != item.EquipmentType) return;
           // if (uiSlot.EquipmentIcon == null || uiSlot.UnequippedIcon == null) return;
            
            ToggleUISlot(uiSlot, true);
            uiSlot.EquipmentIcon.GetComponent<Image>().sprite = item.ItemSprite;

            GameObject iconGO = uiSlot.EquipmentIcon;

            if (iconGO.GetComponent<Button>() != null)
            {
                Button button = iconGO.GetComponent<Button>();
                
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UnequipItemAction(uiSlot, item));
            }
            else
            {
                uiSlot.EquipmentIcon.AddComponent<Button>().onClick.AddListener(() => UnequipItemAction(uiSlot, item));    
            }
            
        });
    }

    private void UnequipItemAction(UIEquipmentSlot uiSlot, EquipmentItem item)
    {
        Destroy(uiSlot.EquipmentIcon.GetComponent<Button>());
        ToggleUISlot(uiSlot, false);
        FindObjectOfType<InventoryManager>().AddItem(item, 1);
        FindObjectOfType<EquipmentManager>().UnassignEquipmentItem(uiSlot.EquipmentType);
    }

    private void ToggleUISlot(UIEquipmentSlot uiSlot, bool hasEquipmentInSlot)
    {
        uiSlot.UnequippedIcon.SetActive(!hasEquipmentInSlot);
        uiSlot.EquipmentIcon.SetActive(hasEquipmentInSlot);
    }

    private void OnDestroy()
    {
        allSlots.ForEach(uiSlot =>
        {
            uiSlot.EquipmentIcon.GetComponentInChildren<Button>()?.onClick.RemoveAllListeners();
        });
    }
}

// LIstener på equipped item referer til det et item der er blevet destroyed. Listener skal genskabes ved start.