using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        LoadEquipment();
    }

    private void LoadEquipment()
    {
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
            
            ToggleUISlot(uiSlot, true);
            SetTooltip(uiSlot, item.TooltipInfo);
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
    
    public void SetTooltip(UIEquipmentSlot slot, TooltipInfo content)
    {
        UITooltipTrigger uiTooltipTrigger = slot.EquipmentIcon.gameObject.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = content.Title;
        uiTooltipTrigger.subtitle = content.Subtitle;
        uiTooltipTrigger.body = content.Body;
    }

}

// LIstener på equipped item referer til det et item der er blevet destroyed. Listener skal genskabes ved start.