using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatLoader : MonoBehaviour
{
    // Positions for spawning
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;

    [Header("Enemies")] 
    [SerializeField] private List<GameObject> enemies;

    [Header("Background")] 
    [SerializeField] private GameObject backgroundGO;
    [SerializeField] private List<Sprite> backgroundSprites;

    [Header("UI Elements")] 
    [SerializeField] private GameObject statDisplayPrefab;
    [SerializeField] private GameObject playerStatDisplayContainer;
    [SerializeField] private GameObject statDisplayContainer;
    [SerializeField] private float basicOffset = 60;
    [SerializeField] private float widthPrStatDisplay = 110;
    
    private void Awake()
    {
        SetupBackground();
    }

    // Amount should come from battlestarter script.
    public void ResizeStatDisplayContainer()
    {
        int amountOfDisplays = FindObjectOfType<CombatSystem>().AmountToSpawn;
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
        var spawnCombatUnit = SpawnCombatUnit(playerPrefab, playerStation, 1);
        AddStatDisplayForPlayer(spawnCombatUnit.GetComponent<CombatUnit>());
        return spawnCombatUnit;;
    }
    
    // Get level and enemybases pool from gamemanager
    public GameObject SpawnEnemy(Transform station, int level)
    {
        var spawnCombatUnit = SpawnCombatUnit(GetRandomEnemyPrefab(), station, level);
        AddStatDisplayForEnemyUnit(spawnCombatUnit.GetComponent<CombatUnit>());
        return spawnCombatUnit;;
    }

    public GameObject SpawnCombatUnit(GameObject unitPrefab, Transform station, int level)
    {
        GameObject spawnedUnit = Instantiate(unitPrefab, station);
        spawnedUnit.GetComponentInChildren<SpriteRenderer>().sprite = unitPrefab.GetComponent<CombatUnit>().IdleSprite;
        spawnedUnit.GetComponent<CombatUnit>().InitiateCurrentStatsForCombat(level);
        return spawnedUnit;
    }

    // This shouldnt be random, but should be selected from a pool depending on gamemanager
    private GameObject GetRandomEnemyPrefab()
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
