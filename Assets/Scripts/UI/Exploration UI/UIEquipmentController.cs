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
    private List<UIEquipmentSlot> allSlots;

    private void Start()
    {
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
    }

    public void UpdateSelectedSlotOnEquip(EquipmentItem item)
    {
        allSlots.ForEach(uiSlot =>
        {
            if (uiSlot.EquipmentType != item.EquipmentType) return;
            
            ToggleUISlot(uiSlot, true);
            uiSlot.EquipmentIcon.sprite = item.ItemSprite;
            uiSlot.EquipmentIcon.gameObject.AddComponent<Button>().onClick.AddListener(() => UnequipItemAction(uiSlot, item));
        });
    }

    public void UpdateSelectedSlotOnUnequip(EquipmentItem item)
    {
        allSlots.ForEach(uiSlot =>
        {
            if (uiSlot.EquipmentType != item.EquipmentType) return;

            UnequipItemAction(uiSlot, item);

        });
    }

    private void UnequipItemAction(UIEquipmentSlot uiSlot, EquipmentItem item)
    {
        Destroy(uiSlot.EquipmentIcon.gameObject.GetComponent<Button>());
        ToggleUISlot(uiSlot, false);
        FindObjectOfType<InventoryManager>().AddItem(item, 1);
        FindObjectOfType<EquipmentManager>().UnassignEquipmentItem(uiSlot.EquipmentType);
    }

    private void ToggleUISlot(UIEquipmentSlot uiSlot, bool hasEquipmentInSlot)
    {
        uiSlot.UnequippedIcon.gameObject.SetActive(!hasEquipmentInSlot);
        uiSlot.EquipmentIcon.gameObject.SetActive(hasEquipmentInSlot);
    }
    
}
