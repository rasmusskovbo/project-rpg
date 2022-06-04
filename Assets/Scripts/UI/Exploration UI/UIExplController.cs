using System.Collections.Generic;
using System.Linq;
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
    
    [Header("Skills Screen")]
    [SerializeField] private GameObject skills;
    [SerializeField] private Image skillsButtonImage;
    
    [Header("Talents Screen")]
    [SerializeField] private GameObject talents;
    [SerializeField] private Image talentsButtonImage;
    
    [Header("Quest Screen")]
    [SerializeField] private GameObject quests;
    [SerializeField] private Image questsButtonImage;
    
    [Header("Inventory Screen")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private Image inventoryStatsButtonImage;
    
    [Header("Pause Screen")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image pauseMenuButtonImage;

    private bool areAllElementsHidden = true;
    private bool firstSetup = true;
    private bool pauseGame;
    
    private EventSystem eventSystem;
    private List<GameObject> uiObjects;
    private List<Image> uiHUDIcons;
    
    private void Start()
    {
        inventory.GetComponent<UIInventoryController>().InitInventoryUI();
        eventSystem = FindObjectOfType<EventSystem>();
        GameEvents.Instance.onShowDialog += () =>
        {
            FindObjectOfType<UIExplController>().CloseUIOnDialog();
        };
        uiObjects = new List<GameObject>
        {
            characterStats, inventory, quests, skills, pauseMenu, talents
        };
        uiHUDIcons = new List<Image>()
        {
            characterStatsButtonImage, inventoryStatsButtonImage, questsButtonImage, skillsButtonImage, pauseMenuButtonImage, talentsButtonImage
        };
        
        HideUI();
    }

    public void HideUI()
    {
        if (firstSetup)
        {
            if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].SetActive(false);
                uiHUDIcons[i].sprite = inactiveUIButton;
            }
            
            firstSetup = false;
        }
        else
        {
            areAllElementsHidden = uiObjects.All(obj => !obj.activeSelf);
            
            if (areAllElementsHidden)
            {
                FindObjectOfType<GameManager>().ExplorationState = ExplorationState.Pause;
                TogglePauseMenu();
            }
            else
            {
                if (uiObjects.Count != uiHUDIcons.Count) Debug.Log("UI -> Object List and Image List not the same size");

                for (int i = 0; i < uiObjects.Count; i++)
                {
                    uiObjects[i].SetActive(false);
                    uiHUDIcons[i].sprite = inactiveUIButton;
                }
                FindObjectOfType<GameManager>().ExplorationState = ExplorationState.Explore;
            }
        }
    }

    public void CloseUIOnDialog()
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
    
    public void ToggleSkills()
    {
        skills.SetActive(!skills.activeSelf);
        skillsButtonImage.sprite =
            skills.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }
    
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseMenuButtonImage.sprite =
            pauseMenu.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }

    public void ToggleTalentsMenu()
    {
        talents.SetActive(!talents.activeSelf);
        talentsButtonImage.sprite =
            talents.activeSelf ? activeUIButton : inactiveUIButton;
        eventSystem.SetSelectedGameObject(null);
    }
}
