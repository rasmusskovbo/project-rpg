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
    private CombatUnit player;
    private GameObject playerGO;
    private int remainingPlayerActions = 0;

    // Enemies
    [SerializeField] private Transform topEnemyStation;
    [SerializeField] private Transform centerEnemyStation;
    [SerializeField] private Transform bottomEnemyStation;
    [SerializeField] private Transform frontTopEnemyStation;
    [SerializeField] private Transform frontBottomEnemyStation;
    private CombatUnit topEnemy;
    private CombatUnit centerEnemy;
    private CombatUnit bottomEnemy;
    private CombatUnit frontTopEnemy;
    private CombatUnit frontBottomEnemy;
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
        StartCoroutine(SetupCombat());
    }

    /*
     * Setup combat scene
     * Instantiate and init players, enemies and systems.
     * Then call SetNextState to begin combat.
     */
    IEnumerator SetupCombat()
    {
        state = CombatState.START;
        
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
   
    void SetupPlayer()
    {
        playerGO = combatLoader.SpawnPlayer(playerStation);
        player = playerGO.GetComponent<CombatUnit>();
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
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
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
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();
                
                break;
            case 3:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemyStation.gameObject.SetActive(false);
                frontBottomEnemyStation.gameObject.SetActive(false);
                
                break;
            case 4:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();
                
                break;
            case 5:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemy = centerEnemyGO.GetComponent<CombatUnit>();
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();
                
                break;
        }
    }
    
    /*
     * Updates the CombatState depending on the state of the scene.
     * This is the core of state management for combat.
     */
    private void SetNextState()
    {
        if (player.isActiveAndEnabled)
        {
            //GetActiveEnemies().ForEach(go => Debug.Log(go.name));
            
            if (GetActiveEnemies().Count > 0) {

                Debug.Log("remaining pas: " + remainingPlayerActions);
                if (remainingPlayerActions > 0)
                {
                    NextPlayerAction();
                    return;
                } 
                
                CombatUnit nextToAct = turnManager.GetNextTurn();

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

    /*
     * Checks and returns all enemies and their stations.
     * Disable all inactive stations.
     */
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

    public List<CombatUnit> GetTargetAndActiveAdjacentPosition(CombatUnit target)
    {
        List<CombatUnit> allPositions = new List<CombatUnit>();
        
        allPositions.Add(topEnemy);
        allPositions.Add(frontTopEnemy);
        allPositions.Add(centerEnemy);
        allPositions.Add(frontBottomEnemy);
        allPositions.Add(bottomEnemy);

        var indexOfTarget = allPositions.FindIndex(position => position.Equals(target));

        List<CombatUnit> targetAndActiveAdjacentTargets = new List<CombatUnit> {target};
        
        if (indexOfTarget < 3)
        {
            Debug.Log("Should add south enemy to list: " + shouldAddToList(allPositions[indexOfTarget + 1]));
            if (shouldAddToList(allPositions[indexOfTarget + 1]))
            {
                targetAndActiveAdjacentTargets.Add(allPositions[indexOfTarget + 1]);
            } else if (shouldAddToList(allPositions[indexOfTarget + 2]))
            {
                targetAndActiveAdjacentTargets.Add(allPositions[indexOfTarget + 2]);
            }
            
        }
        else
        {
            if (shouldAddToList(allPositions[indexOfTarget - 1]))
            {
                targetAndActiveAdjacentTargets.Add(allPositions[indexOfTarget - 1]);
            }
            else if (shouldAddToList(allPositions[indexOfTarget - 2]))
            {
                targetAndActiveAdjacentTargets.Add(allPositions[indexOfTarget - 2]);
            }  
        }
        
        return targetAndActiveAdjacentTargets;
    }

    private bool shouldAddToList(CombatUnit enemy)
    {
        return enemy && enemy.isActiveAndEnabled;
    }
    
    /*
     * Resets respective game elements for next player action
     * Called just before the player gets the action by SetNextState
     */
    void NextPlayerAction()
    {
        combatLog.NextPlayerAction(remainingPlayerActions);
        uiInputController.MoveCursorToDefaultActionSelect();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }
    
    /*
     * Resets respective game elements for the players turn.
     * Called just before the player's turn by SetNextState
     */
    void NewPlayerTurn()
    {
        player.GetComponent<CombatEffectManager>().ProcessActiveEffects();
        remainingPlayerActions = maxPlayerActions;
        skillManager.DecreaseCooldowns();
        combatLog.PlayerTurn();
        uiInputController.ResetActionSelectUI();
        uiInputController.PlayerHasChosenATarget = false;
        state = CombatState.PLAYER_ACTION_SELECT;
    }
    
    /*
     * Resets respective game elements for the enemy's turn
     * Called just before the enemy's turn by SetNextState
     */
    void NewEnemyTurn(CombatUnit enemy)
    {
        enemy.GetComponent<CombatEffectManager>().ProcessActiveEffects();
        combatLog.EnemyTurn(enemy);
        state = CombatState.ENEMY_TURN;
        StartCoroutine(ProcessEnemyTurn(enemy));
    }
    
    /*
     * When the player selects a skill
     * Called by InputController
     */
    public void OnSkillSelect(CombatMove move)
    {
        if (state != CombatState.PLAYER_SKILL_SELECT) return;
        
        chosenSkill = move;

        // If the move has been executed, and does not have targets, else go to target select.
        if (DoesMoveNeedTargets(chosenSkill))
        {
            state = CombatState.PLAYER_TARGET_SELECT; 
        }
        else
        {
            UpdateRemainingActions();
            StartCoroutine(UsePlayerSkillWithoutTargetSelection(move));
        }

        // Do something with coroutines here or in skillexec.
    }
    
    private bool DoesMoveNeedTargets(CombatMove move)
    {
        if (move.GetTargets().Equals(CombatMoveTargets.Self) || move.GetTargets().Equals(CombatMoveTargets.Global))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    /*
     * When the player selects a target (if skill is not self or globally targetted)
     * Called by InputController
     */
    public void OnTargetSelect(int targetIndex)
    {
        if (state != CombatState.PLAYER_TARGET_SELECT) return;

        UpdateRemainingActions();
        
        // See TO DO note below
        StartCoroutine(UsePlayerSkillWithTarget(chosenSkill, GetActiveEnemies()[targetIndex].GetComponent<CombatUnit>()));
        
    }
    
    private void UpdateRemainingActions()
    {
        if (remainingPlayerActions > 0)
        {
            remainingPlayerActions--;
            uiInputController.DisableChosenAction(chosenSkill.GetActionType());
        }
    }
    
    public void CheckForDeath(TakeDamageResult result)
    {
        if (result.IsUnitDead)
        {
            Destroy(result.Unit.gameObject);
            if (result.Unit.UnitType == UnitType.PLAYER) playerStation.gameObject.SetActive(false);
            
            combatLog.PrintToLog(result.Unit.UnitName + " died!");
            
            uiInputController.UpdateTargetablePositions();
            turnManager.RemoveFromActiveUnits(result.Unit);
        }
    }
    
    IEnumerator UsePlayerSkillWithoutTargetSelection(CombatMove move)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        List<TakeDamageResult> results = skillExecutor.ExecuteMove(move, player, player, GetActiveEnemies());
        
        yield return new WaitForSeconds(2); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'

        results?.ForEach(CheckForDeath);
        
        // Check for death of targets if global targets
        //CheckForDeath(player, result);
        
        SetNextState();
    }
    
    IEnumerator UsePlayerSkillWithTarget(CombatMove move, CombatUnit target)
    {
        /* Apply Skill ->
            Animation through facade (abstraction layer)
            VFX (particle effects etc) - Maybe this is animation layer as well
            Combat calculations
        */
        List<TakeDamageResult> results = skillExecutor.ExecuteMove(move, player, target, GetActiveEnemies());

        yield return new WaitForSeconds(2); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'
        
        results?.ForEach(CheckForDeath);
        
        SetNextState();
    }
    
    
    /*
     * Simple test impl of enemy AI
     * Will be delegated to EnemyController
     */
    private IEnumerator ProcessEnemyTurn(CombatUnit enemy)
    {
        TakeDamageResult result = skillExecutor.ExecuteMoveOnTarget(skillManager.GetTestMove(),enemy, player);
        //combatLog.DamagePlayer(enemy, skillManager.GetTestMove(), result.DamageTaken);
        combatLog.PrintToLog("New player HP: " + player.CurrentHp);
        
        yield return new WaitForSeconds(1f);
        
        CheckForDeath(result);
        SetNextState();
    }
    
    // Properties
    public CombatState State
    {
        get => state;
        set => state = value;
    }
    
}
