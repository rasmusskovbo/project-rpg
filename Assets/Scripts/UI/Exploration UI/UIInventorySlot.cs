using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button slotButton;

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
        slotButton.onClick.AddListener(() => onClickCallback());
    }

    public void SetTooltip(TooltipInfo content)
    {
        UITooltipTrigger uiTooltipTrigger = this.gameObject.AddComponent<UITooltipTrigger>();
        uiTooltipTrigger.title = content.Title;
        uiTooltipTrigger.subtitle = content.Subtitle;
        uiTooltipTrigger.body = content.Body;
    }

}
