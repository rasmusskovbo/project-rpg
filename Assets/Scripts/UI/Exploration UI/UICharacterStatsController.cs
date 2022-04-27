using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterStatsController : MonoBehaviour
{
    private GameManager gameManager;
    
    [Header("Header Panel")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI currentExp;
    [SerializeField] private TextMeshProUGUI nextLvlExp;
    [SerializeField] private TextMeshProUGUI currentHp;
    [SerializeField] private TextMeshProUGUI maxHp;
    [SerializeField] private Slider hpSlider;
    
    [Header("Stat Panel")]
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI intellect;
    [SerializeField] private TextMeshProUGUI attackPower;
    [SerializeField] private TextMeshProUGUI abilityPower;
    [SerializeField] private TextMeshProUGUI physCritChance;
    [SerializeField] private TextMeshProUGUI magicCritChance;
    [SerializeField] private TextMeshProUGUI physicalDefense;
    [SerializeField] private TextMeshProUGUI magicalDefense;
    [SerializeField] private TextMeshProUGUI blockPower;
    [SerializeField] private TextMeshProUGUI dodgeChance;
    [SerializeField] private TextMeshProUGUI speed;

    [Header("Level Up UI")]
    [SerializeField] private GameObject levelUpText;
    [SerializeField] private TextMeshProUGUI statPoints;
    [SerializeField] private GameObject strengthUp;
    [SerializeField] private GameObject agilityUp;
    [SerializeField] private GameObject intellectUp;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateStats();
        ToggleLevelUpButtons(false);
    }

    private void OnEnable()
    {
        UpdateStats();
    }

    private void Update()
    {
        UpdateStats();
        
        if (gameManager.PlayerData.remainingStatPoints <= 0)
        {
            ToggleLevelUpButtons(false);
            return;
        }
        
        ToggleLevelUpButtons(true);
       
    }

    public void UpdateStats()
    {
        if (!this.isActiveAndEnabled) return;
        
        UnitBase unitBase = gameManager.PlayerCombatBase;
        PlayerData playerData = gameManager.PlayerData;
        CombatUnit unit = this.AddComponent<CombatUnit>(); // todo Get this from gamemanager.
        unit.InitiateUnit(unitBase, playerData.level);
        
        // Header
        unitName.text = unit.UnitName;
        level.text = "Lv" + playerData.level;
        currentExp.text = playerData.exp.ToString();
        nextLvlExp.text = playerData.nextLvLExp.ToString();
        currentHp.text = playerData.currentHp.ToString();
        maxHp.text = unit.MaxHp.ToString();
        hpSlider.minValue = 0;
        hpSlider.maxValue = unit.MaxHp;
        hpSlider.value = playerData.currentHp;
        
        // Stat panel
        statPoints.text = playerData.remainingStatPoints.ToString();
        
        strength.text = unit.Strength.ToString();
        agility.text = unit.Agility.ToString();
        intellect.text = unit.Intellect.ToString();
        attackPower.text = unit.AttackPower.ToString();
        abilityPower.text = unit.AbilityPower.ToString();
        physCritChance.text = Math.Round(unit.PhysicalCritChance * 100) + "%";
        magicCritChance.text = Math.Round(unit.MagicalCritChance * 100) + "%";
        physicalDefense.text = unit.PhysicalDefense.ToString();
        magicalDefense.text = unit.MagicalDefense.ToString();
        blockPower.text = unit.PhysicalBlockPower.ToString();
        dodgeChance.text = Math.Round(unit.DodgeChance * 100) + "%";
        speed.text = unit.Speed.ToString();
        
    }

    private void ToggleLevelUpButtons(bool isActive)
    {
        levelUpText.SetActive(isActive);
        statPoints.gameObject.SetActive(isActive);
        strengthUp.SetActive(isActive);
        agilityUp.SetActive(isActive);
        intellectUp.SetActive(isActive);
    }

    public void AddStatPoint(int typeIndex)
    {
        gameManager.AddStatPoint((StatType) typeIndex);
    }
}
