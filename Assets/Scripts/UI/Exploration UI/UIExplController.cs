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
    
    [Header("Quest Screen")]
    [SerializeField] private GameObject quests;
    [SerializeField] private Image questsButtonImage;

    private EventSystem eventSystem;
    private List<GameObject> uiObjects;
    private List<Image> uiHUDIcons;
    
    private void Start()
    {
        inventory.GetComponent<UIInventoryController>().InitInventoryUI();
        eventSystem = FindObjectOfType<EventSystem>();
        FindObjectOfType<GameEvents>().onShowDialog += () =>
        {
            HideUI();
        };
        uiObjects = new List<GameObject>
        {
            characterStats, inventory, quests
        };
        uiHUDIcons = new List<Image>()
        {
            characterStatsButtonImage, inventoryStatsButtonImage, questsButtonImage
        };
        
        HideUI();
    }

    public void HideUI()
    {
        if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(false);
            uiHUDIcons[i].sprite = inactiveUIButton;
        }
        
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
    
    public void ToggleQuests() 
    {
        quests.SetActive(!quests.activeSelf);
        questsButtonImage.sprite =
            quests.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }
}
