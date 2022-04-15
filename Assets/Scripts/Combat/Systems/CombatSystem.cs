using System.Collections;
using System.Collections.Generic;
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
    private TurnManager turnManager;
    private EnemyController enemyController;
    private SkillManager skillManager;
    private SkillExecutor skillExecutor;
    
    // Player
    [SerializeField] private Transform playerStation;
    [SerializeField] private int maxPlayerActions = 2;
    private PlayerCombat player;
    private GameObject playerGO;
    private int remainingPlayerActions = 2;

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
        skillManager = FindObjectOfType<SkillManager>();
        skillExecutor = FindObjectOfType<SkillExecutor>();
        enemyGameObjects = new List<GameObject>();
        remainingPlayerActions = maxPlayerActions;
        StartCoroutine(SetupCombat());
    }

    IEnumerator SetupCombat()
    {
        SetupPlayer();
        SetupEnemies();
        SetupTurnManager();

        combatLog.Clear();
        combatLog.StartOfCombat();

        yield return new WaitForSecondsRealtime(1);
        
        SetNextState();
    }

    private void SetupTurnManager()
    {
        List<GameObject> activeEnemies = GetActiveEnemies();
        activeEnemies.Add(playerGO);
        turnManager = new TurnManager(activeEnemies);
    }
    
    private void SetNextState()
    {
        if (player.isActiveAndEnabled)
        {
            //GetActiveEnemies().ForEach(go => Debug.Log(go.name));
            
            if (GetActiveEnemies().Count > 0) {

                if (remainingPlayerActions > 0)
                {
                    NextPlayerAction();
                    return;
                }
                
                Unit nextToAct = turnManager.GetNextTurn();

                if (nextToAct.UnitType == UnitType.PLAYER)
                {
                    NewPlayerTurn();
                }
                else if (nextToAct.UnitType == UnitType.ENEMY)
                {
                    NewEnemyTurn(nextToAct);
                }
            }
            else
            {
                state = CombatState.VICTORY;
            }  
        }
        else
        {
            state = CombatState.DEFEAT;
        }

        Debug.Log("Current state: " + state);
    }

    void SetupPlayer()
    {
        playerGO = combatLoader.SpawnPlayer(playerStation);
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
    
    void NextPlayerAction()
    {
        combatLog.NextPlayerAction(remainingPlayerActions);
        uiInputController.MoveCursorToDefaultActionSelect();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }
    
    void NewPlayerTurn()
    {
        remainingPlayerActions = maxPlayerActions;
        skillManager.DecreaseCooldowns();
        combatLog.PlayerTurn();
        uiInputController.ResetActionSelectUI();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }
    
    void NewEnemyTurn(Unit enemy)
    {
        combatLog.PrintToLog(enemy.UnitName + "'s turn!");
        state = CombatState.ENEMY_TURN;
        StartCoroutine(ProcessEnemyTurn());
    }
    
    public void OnSkillSelect(CombatMove move)
    {
        if (state != CombatState.PLAYER_SKILL_SELECT) return;
        
        chosenSkill = move;

        if (skillExecutor.ExecuteMove(move))
        {
            state = CombatState.PLAYER_TARGET_SELECT;    
        }
        
        state = CombatState.PLAYER_TARGET_SELECT;    
        // Do something with coroutines here or in skillexec.
    }

    // Maybe better to pass a target index (1-3) than to pass Enemies around and let Combat System handle enemy data.
    public void OnTargetSelect(int targetIndex)
    {
        if (state != CombatState.PLAYER_TARGET_SELECT) return;

        StartCoroutine(UsePlayerSkill(chosenSkill, GetActiveEnemies()[targetIndex].GetComponent<Unit>()));

        if (remainingPlayerActions > 0)
        {
            remainingPlayerActions--;
            uiInputController.DisableChosenAction(chosenSkill.GetActionType());    
        }
        
    }

    IEnumerator UsePlayerSkill(CombatMove move, Unit target)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        FindObjectOfType<SkillManager>().PutCombatMoveOnCooldown(move);
        TakeDamageResult result = target.TakePhysicalDamage(move.GetPower()); // test needs skillhandler to provide decide whether skill is physical etc
        combatLog.PlayerUsedCombatMove(move, target, result.DamageTaken);

        yield return new WaitForSeconds(2); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'
        
        // Check for death of target.
        CheckForDeath(target, result);
        
        SetNextState();
    }
    /*
     * Simple test impl of enemy AI
     */
    private IEnumerator ProcessEnemyTurn()
    {
        TakeDamageResult result = player.TakePhysicalDamage(15); 
        combatLog.PrintToLog("Player hit for : " + result.DamageTaken + ". Player HP: " + player.CurrentHp);
        yield return new WaitForSeconds(1);
        
        CheckForDeath(player, result);
        SetNextState();
    }

    public void CheckForDeath(Unit target, TakeDamageResult result)
    {
        if (result.IsUnitDead)
        {
            Destroy(target.gameObject);
            if (target.UnitType == UnitType.PLAYER) playerStation.gameObject.SetActive(false);
            
            combatLog.PrintToLog(target.UnitName + " died!");
            
            uiInputController.UpdateTargetablePositions();
            turnManager.RemoveFromActiveUnits(target);
        }
    }

    public CombatState State
    {
        get => state;
        set => state = value;
    }

    public PlayerCombat Player
    {
        get => player;
    }
}
