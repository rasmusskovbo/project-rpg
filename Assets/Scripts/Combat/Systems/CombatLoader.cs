using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatLoader : MonoBehaviour
{
    // Positions for spawning
    [Header("Player")] 
    [SerializeField] private UnitBase player;
    [SerializeField] private GameObject playerPrefab;

    [Header("Enemies")] 
    [SerializeField] private List<UnitBase> enemies;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Background")] 
    [SerializeField] private GameObject backgroundGO;
    [SerializeField] private List<Sprite> backgroundSprites;

    [Header("UI Elements")] 
    [SerializeField] private GameObject statDisplayPrefab;
    [SerializeField] private GameObject playerStatDisplayContainer;
    [SerializeField] private GameObject statDisplayContainer;
    [SerializeField] private float basicOffset = 60;
    [SerializeField] private float widthPrStatDisplay = 110;

    private GameManager gameManager;
    
    private void Awake()
    {
        SetupBackground();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Amount should come from battlestarter script.
    public void ResizeStatDisplayContainer()
    {
        int amountOfDisplays = FindObjectOfType<CombatEncounterManager>().AmountOfEnemiesToSpawn;
        float width = 500 - basicOffset - (widthPrStatDisplay * (amountOfDisplays - 1)); 
        statDisplayContainer.GetComponent<RectTransform>().offsetMax = new Vector2(-width , 0);
    }
    
    public void AddStatDisplayForPlayer(CombatUnit unit)
    {
        Instantiate(statDisplayPrefab, playerStatDisplayContainer.transform)
            .GetComponent<UIStatDisplay>().ConnectedUnit = unit;
        ResizeStatDisplayContainer();
    }
    
    public void AddStatDisplayForEnemyUnit(CombatUnit unit)
    {
        Instantiate(statDisplayPrefab, statDisplayContainer.transform)
            .GetComponent<UIStatDisplay>().ConnectedUnit = unit;
        ResizeStatDisplayContainer();
    }
    
    // Currently player is hardcoded to prefab and lvl 1. CombatUnit returned to system should be with active stats.
    public GameObject SpawnPlayer(Transform playerStation)
    {
        UnitBase playerBase = gameManager.PlayerData.unitBase;
        var spawnCombatUnit = SpawnCombatUnit(playerBase, playerPrefab, playerStation, 1);
        AddStatDisplayForPlayer(spawnCombatUnit.GetComponent<CombatUnit>());
        spawnCombatUnit.GetComponent<CombatUnit>().CurrentHp = gameManager.PlayerData.currentHp;
        return spawnCombatUnit;;
    }
    
    // Get level and enemybases pool from gamemanager
    public GameObject SpawnEnemy(Transform station, int level)
    {
        var spawnCombatUnit = SpawnCombatUnit(GetRandomEnemyBase(), enemyPrefab, station, level);
        AddStatDisplayForEnemyUnit(spawnCombatUnit.GetComponent<CombatUnit>());
        return spawnCombatUnit;;
    }

    public GameObject SpawnCombatUnit(UnitBase unitBase, GameObject unitPrefab, Transform station, int level)
    {
        GameObject spawnedUnit = Instantiate(unitPrefab, station);
        spawnedUnit.GetComponentInChildren<SpriteRenderer>().sprite = unitBase.IdleSprite;
        spawnedUnit.GetComponent<CombatUnit>().InitiateUnit(unitBase, level);
        return spawnedUnit;
    }

    // This shouldnt be random, but should be selected from a pool depending on gamemanager
    private UnitBase GetRandomEnemyBase()
    {
        int randomEnemyIndex = Random.Range(0, enemies.Count);
        return enemies[randomEnemyIndex];
    }

    // This shouldnt be random, but should be selected from a pool depending on gamemanager
    void SetupBackground()
    {
        int randomIndex = Random.Range(0, backgroundSprites.Count);
        backgroundGO.GetComponentsInChildren<SpriteRenderer>()[0].sprite = backgroundSprites[randomIndex];
    }

}
