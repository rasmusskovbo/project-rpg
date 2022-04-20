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
    [SerializeField] private GameObject HpBars;
    [SerializeField] private GameObject hpBarprefab;
    [SerializeField] private float hpBarXOffset;
    [SerializeField] private float hpBarYOffset;
    

    private void Awake()
    {
        SetupBackground();
    }

    /*
    public void AddHpBar(Transform station)
    {
        Vector3 newPosition = station.transform.position;
        newPosition.x += hpBarXOffset;
        newPosition.y += hpBarYOffset;
        
        Instantiate(hpBarprefab, newPosition, Quaternion.identity, HpBars.transform);
    }
    */

    public GameObject SpawnPlayer(Transform playerStation)
    {
        //AddHpBar(playerStation);
        return SpawnCombatUnit(playerPrefab, playerStation, 1);;
    }
    
    // Get level and enemybases pool from gamemanager
    public GameObject SpawnEnemy(Transform station, int level)
    {
        //AddHpBar(station);
        return SpawnCombatUnit(GetRandomEnemyPrefab(), station, level);;
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
