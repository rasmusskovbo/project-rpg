using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class UITooltipController : MonoBehaviour
{
    private static UITooltipController current;
    
    [SerializeField] private UITooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string body, string subtitle = "", string title = "")
    {
        current.tooltip.SetText(body, subtitle, title);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
