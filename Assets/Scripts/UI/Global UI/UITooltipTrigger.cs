using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Multiline()]
    public string body;
    public string subtitle;
    public string title;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        UITooltipController.Show(body, subtitle, title);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltipController.Hide();
    }
}
