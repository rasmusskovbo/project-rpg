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

    private void Awake()
    {
        SetupBackground();
    }

    public GameObject SpawnPlayer(Transform playerStation)
    {
        return SpawnCombatUnit(playerPrefab, playerStation, 1);
    }
    
    // Get level and enemybases pool from gamemanager
    public GameObject SpawnEnemy(Transform station, int level)
    {
        return SpawnCombatUnit(GetRandomEnemyPrefab(), station, level);
    }

    public GameObject SpawnCombatUnit(GameObject unitPrefab, Transform station, int level)
    {
        GameObject spawnedUnit = Instantiate(unitPrefab, station);
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
