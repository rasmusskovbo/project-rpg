using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatLoader : MonoBehaviour
{
    // Positions for spawning
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;

    [Header("Enemies")] 
    [SerializeField] private List<CombatUnit> enemies;
    [SerializeField] private List<EnemyBase> enemyBases; // List of Scriptable Objects to instantiate prefabs from
    [SerializeField] private GameObject enemyPrefab;

    [Header("Background")] 
    [SerializeField] private GameObject backgroundGO;
    [SerializeField] private List<Sprite> backgroundSprites;

    private void Awake()
    {
        SetupBackground();
    }

    public GameObject SpawnPlayer(Transform playerStation)
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        playerGO.GetComponent<PlayerCombat>().InitiatePlayerCombatStats();
        return playerGO;
    }
    
    // Get level and enemybases pool from gamemanager
    public GameObject SpawnEnemy(Transform station, int level)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, station);
        enemyGO.GetComponent<Enemy>().InitiateEnemy(GetRandomEnemyBase(), level);
        
        return enemyGO;
    }

    // This shouldnt be random, but should be selected from a pool depending on gamemanager
    private EnemyBase GetRandomEnemyBase()
    {
        int randomIndex = Random.Range(0, enemyBases.Count);
        return enemyBases[randomIndex];
    }

    // This shouldnt be random, but should be selected from a pool depending on gamemanager
    void SetupBackground()
    {
        int randomIndex = Random.Range(0, backgroundSprites.Count);
        backgroundGO.GetComponentsInChildren<SpriteRenderer>()[0].sprite = backgroundSprites[randomIndex];
    }

}
