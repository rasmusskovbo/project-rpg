using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIExplController : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private Sprite activeUIButton;
    [SerializeField] private Sprite inactiveUIButton;
    
    [Header("Character Stat Screen")]
    [SerializeField] private GameObject characterStats;
    [SerializeField] private Image characterStatsButtonImage;
    
    [Header("Inventory Screen")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private Image inventoryStatsButtonImage;

    private EventSystem eventSystem;

    private void Start()
    {
        inventory.GetComponent<UIInventoryController>().InitInventoryUI();
        characterStats.SetActive(false);
        inventory.SetActive(false);
        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void ToggleCharacterStats()
    {
        characterStats.SetActive(!characterStats.activeSelf);
        characterStatsButtonImage.sprite =
            characterStats.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }

    public void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
        inventoryStatsButtonImage.sprite =
            inventory.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }
}
