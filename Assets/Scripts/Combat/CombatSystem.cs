using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{

    // For spawning - this could be a list of all enemy prefabs chosen randomly.
    
    

    // Positions for spawning
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] private Transform playerStation;
    private PlayerCombat player;

    [Header("Enemies")] 
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<EnemyBase> enemyBases; // List of Scriptable Objects to instantiate prefabs from
    [SerializeField] private Transform topEnemyStation;
    [SerializeField] private Transform centerEnemyStation;
    [SerializeField] private Transform bottomEnemyStation;
    private Enemy topEnemy;
    private Enemy centerEnemy;
    private Enemy bottomEnemy;

    private UICombatLog combatLog;
    public CombatState state;

    private void Awake()
    {
        combatLog = FindObjectOfType<UICombatLog>();
    }

    void Start()
    {
        state = CombatState.START;
        StartCoroutine(SetupCombat());
    }

    IEnumerator SetupCombat()
    {
        // For dynamic content
        GameObject playerGO = Instantiate(playerPrefab, playerStation);

        GameObject enemyGO = Instantiate(enemyPrefab, centerEnemyStation);
        centerEnemy = enemyGO.GetComponent<Enemy>();
        centerEnemy.LoadEnemyScriptableObject(enemyBases[0], 1);

        combatLog.Clear();
        combatLog.PrintToLog("A " + centerEnemy.Name + " has appeared.");

        yield return new WaitForSeconds(2f);
        
        // This should be speed based.
        state = CombatState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        combatLog.PrintToLog("Player's turn!");
    }

    public void OnSkillSelect(CombatMove move)
    {
        combatLog.PrintAttackMove(move, centerEnemy, move.GetPower(), move.GetType().ToString());
    }

}
