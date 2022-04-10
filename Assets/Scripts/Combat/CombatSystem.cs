using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    // Variables
    private CombatState state;
    private CombatMove chosenSkill;
    
    // Systems
    private UICombatLog combatLog;
    private UIPlayerInputController uiInputController;
    private CombatLoader combatLoader;
    private SpeedManager speedManager;
    private EnemyController enemyController;
    
    // Player
    private PlayerCombat player;
    private GameObject playerGO;

    // Enemies
    [SerializeField] private Transform topEnemyStation;
    [SerializeField] private Transform centerEnemyStation;
    [SerializeField] private Transform bottomEnemyStation;
    [SerializeField] private Transform frontTopEnemyStation;
    [SerializeField] private Transform frontBottomEnemyStation;
    private Enemy topEnemy;
    private Enemy centerEnemy;
    private Enemy bottomEnemy;
    private Enemy frontTopEnemy;
    private Enemy frontBottomEnemy;
    private List<GameObject> enemyGameObjects;
    
    // Temporary references
    [SerializeField] private int amountToSpawn;
    [SerializeField] private int levelOfEnemies;
    
    void Awake()
    {
        combatLog = FindObjectOfType<UICombatLog>();
        combatLoader = FindObjectOfType<CombatLoader>();
        uiInputController = FindObjectOfType<UIPlayerInputController>();
        enemyController = FindObjectOfType<EnemyController>();
        enemyGameObjects = new List<GameObject>();
        StartCoroutine(SetupCombat());
    }

    private void Update()
    {
        //Debug.Log("Current combat state: " + state);
    }

    IEnumerator SetupCombat()
    {
        SetupPlayer();
        SetupEnemies();

        List<GameObject> activeEnemies = GetActiveEnemies();
        activeEnemies.Add(playerGO);
        speedManager = new SpeedManager(activeEnemies);
        
        combatLog.Clear();
        combatLog.PrintToLog("Enemies have appeared!");

        yield return new WaitForSeconds(1f);

        GetNextState();
    }

    private void GetNextState()
    {
        Unit nextToAct = speedManager.GetNextTurn();

        if (nextToAct.UnitType == UnitType.PLAYER)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn(nextToAct);
        }
    }

    void SetupPlayer()
    {
        playerGO = combatLoader.SpawnPlayer();
        player = playerGO.GetComponent<PlayerCombat>();
    }
    
    void SetupEnemies()
    {
        // Get amount of enemies from GameManager reference here. Currently hardcoded

        /*
         * This code spawns enemies in predefined patterns, based on the amount of enemies.
         * GameObjects are added to list for UI selections
         * Enemy scripts are added to variables for easier reference for calculations.
         */
        GameObject topEnemyGO;
        GameObject centerEnemyGO;
        GameObject bottomEnemyGO;
        GameObject frontTopEnemyGO;
        GameObject frontBottomEnemyGO;
        
        switch (amountToSpawn)
        {
            case 1:
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);

                topEnemyStation.gameObject.SetActive(false);
                centerEnemy = centerEnemyGO.GetComponent<Enemy>();
                bottomEnemyStation.gameObject.SetActive(false);
                frontTopEnemyStation.gameObject.SetActive(false);
                frontBottomEnemyStation.gameObject.SetActive(false);
                
                break;
            case 2:
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);

                topEnemyStation.gameObject.SetActive(false);
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemyStation.gameObject.SetActive(false);
                frontTopEnemy = frontTopEnemyGO.GetComponent<Enemy>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<Enemy>();
                
                break;
            case 3:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<Enemy>();
                centerEnemy = centerEnemyGO.GetComponent<Enemy>();
                bottomEnemy = bottomEnemyGO.GetComponent<Enemy>();
                frontTopEnemyStation.gameObject.SetActive(false);
                frontBottomEnemyStation.gameObject.SetActive(false);
                
                break;
            case 4:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<Enemy>();
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemy = bottomEnemyGO.GetComponent<Enemy>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<Enemy>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<Enemy>();
                
                break;
            case 5:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<Enemy>();
                centerEnemy = centerEnemyGO.GetComponent<Enemy>();
                bottomEnemy = bottomEnemyGO.GetComponent<Enemy>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<Enemy>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<Enemy>();
                
                break;
        }
    }

    public List<GameObject> GetActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();

        if (shouldAddToList(topEnemy))
        {
            activeEnemies.Add(topEnemy.gameObject);
        }
        else
        {
            topEnemyStation.gameObject.SetActive(false);
        }
        
        if (shouldAddToList(frontTopEnemy))
        {
            activeEnemies.Add(frontTopEnemy.gameObject);
        }
        else
        {
            frontTopEnemyStation.gameObject.SetActive(false);
        }

        if (shouldAddToList(centerEnemy))
        {
            activeEnemies.Add(centerEnemy.gameObject);
        }
        else
        {
            centerEnemyStation.gameObject.SetActive(false);
        }
        
        if (shouldAddToList(frontBottomEnemy))
        {
            activeEnemies.Add(frontBottomEnemy.gameObject);
        }
        else
        {
            frontBottomEnemyStation.gameObject.SetActive(false);
        }

        if (shouldAddToList(bottomEnemy))
        {
            activeEnemies.Add(bottomEnemy.gameObject);
        }
        else
        {
            bottomEnemyStation.gameObject.SetActive(false);
        }
        
        return activeEnemies;
    }

    private bool shouldAddToList(Enemy enemy)
    {
        return enemy && enemy.isActiveAndEnabled;
    }
    
    void PlayerTurn()
    {
        combatLog.PrintToLog("Player's turn!");
        state = CombatState.PLAYER_ACTION_SELECT;
    }
    
    void EnemyTurn(Unit enemy)
    {
        combatLog.PrintToLog(enemy.UnitName + "'s turn!");
        state = CombatState.ENEMY_TURN;
        StartCoroutine(ProcessEnemyTurn());
    }

    private IEnumerator ProcessEnemyTurn()
    {
        player.TakePhysicalDamage(5);
        yield return new WaitForSeconds(1);
        GetNextState();
    }

    public void OnSkillSelect(CombatMove move)
    {
        if (state != CombatState.PLAYER_SKILL_SELECT) return;
        
        chosenSkill = move;
        state = CombatState.PLAYER_TARGET_SELECT;
        
    }

    // Maybe better to pass a target index (1-3) than to pass Enemies around and let Combat System handle enemy data.
    public void OnTargetSelect(int targetIndex)
    {
        if (state != CombatState.PLAYER_TARGET_SELECT) return;

        StartCoroutine(UsePlayerSkill(chosenSkill, GetActiveEnemies()[targetIndex].GetComponent<Unit>()));
        
    }

    IEnumerator UsePlayerSkill(CombatMove move, Unit target)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        TakeDamageResult result = target.TakePhysicalDamage(move.GetPower()); // test needs skillhandler to provide decide whether skill is physical etc
        combatLog.PrintAttackMove(move, target, result.DamageTaken);
        
        yield return new WaitForSeconds(2); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'
        
        if (result.IsUnitDead) {
            Destroy(target.gameObject); // test
            combatLog.PrintToLog("Target died!");
            //Destroy(topEnemyStation.gameObject);// find and disable station instead
            uiInputController.UpdateTargetablePositions();
        }
        
        // Relinquish control to SpeedManager
        // temporary AI turn
        // temporary playerturn again
        GetNextState();
    }

    public CombatState State
    {
        get => state;
        set => state = value;
    }
}
