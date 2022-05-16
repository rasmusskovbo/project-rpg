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
    private List<GameObject> uiObjects;
    private List<Image> uiImages;
    
    private void Start()
    {
        inventory.GetComponent<UIInventoryController>().InitInventoryUI();
        characterStats.SetActive(false);
        inventory.SetActive(false);
        eventSystem = FindObjectOfType<EventSystem>();
        GameEvents.Instance.onShowDialog += () =>
        {
            HideUI();
        };

        uiObjects = new List<GameObject>
        {
            characterStats, inventory
        };
        uiImages = new List<Image>()
        {
            characterStatsButtonImage, inventoryStatsButtonImage
        };
    }

    private void HideUI()
    {
        if (uiObjects.Count != uiImages.Count) Debug.Log("UI -> Object List and Image List not the same size");

        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(false);
            uiImages[i].sprite = inactiveUIButton;
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
}
