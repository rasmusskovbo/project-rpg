using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button slotButton;
    
    // here ondestroy
    
    public void InitSlotVisuals(Sprite itemSprite, int count)
    {
        itemImage.sprite = itemSprite;
        UpdateSlotCount(count);
    }

    public void UpdateSlotCount(int count)
    {
        countText.text = count.ToString();
    }

    public void AssignSlotButtonCallback(System.Action onClickCallback)
    {
        Debug.Log("Assigned callback");
        slotButton.onClick.AddListener(() => onClickCallback());
    }

}
