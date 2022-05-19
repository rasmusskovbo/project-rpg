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
    [Header("Player")]
    [SerializeField] private Transform playerStation;
    [SerializeField] private int maxPlayerActions = 2;
    [SerializeField] private float playerCombatAnimationSpeed = 1.5f;
    private CombatUnit player;
    private GameObject playerGO;
    private int remainingPlayerActions = 0;

    // Enemies
    [Header("Enemies")]
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

        CombatEncounterManager combatEncounterManager = FindObjectOfType<CombatEncounterManager>();
        int amountToSpawn = combatEncounterManager.AmountOfEnemiesToSpawn;
        int levelOfEnemies = combatEncounterManager.EnemyLvl;
        
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
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                
                topEnemy = topEnemyGO.GetComponent<CombatUnit>();
                centerEnemyStation.gameObject.SetActive(false);
                bottomEnemy = bottomEnemyGO.GetComponent<CombatUnit>();
                frontTopEnemy = frontTopEnemyGO.GetComponent<CombatUnit>();
                frontBottomEnemy = frontBottomEnemyGO.GetComponent<CombatUnit>();
                
                break;
            case 5:
                topEnemyGO = combatLoader.SpawnEnemy(topEnemyStation, levelOfEnemies);
                frontTopEnemyGO = combatLoader.SpawnEnemy(frontTopEnemyStation, levelOfEnemies);
                centerEnemyGO = combatLoader.SpawnEnemy(centerEnemyStation, levelOfEnemies);
                frontBottomEnemyGO = combatLoader.SpawnEnemy(frontBottomEnemyStation, levelOfEnemies);
                bottomEnemyGO = combatLoader.SpawnEnemy(bottomEnemyStation, levelOfEnemies);
                
                
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
                StartCoroutine(VictorySequence());
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
        List<CombatUnit> enemyReferences = GetAllEnemyReferences();
        List<Transform> enemyStations = GetAllEnemyStations();
        List<GameObject> activeEnemies = new List<GameObject>();

        for (int i = 0; i < enemyReferences.Count; i++)
        {
            if (shouldAddToList(enemyReferences[i]))
            {
                activeEnemies.Add(enemyReferences[i].gameObject);
            }
            else
            {
                enemyStations[i].gameObject.SetActive(false);
            }
        }

        return activeEnemies;
    }

    private List<Transform> GetAllEnemyStations()
    {
        return new List<Transform>
        {
            topEnemyStation,
            frontTopEnemyStation,
            centerEnemyStation,
            frontBottomEnemyStation,
            bottomEnemyStation
        };
    }

    public List<CombatUnit> GetTargetAndActiveAdjacentPosition(CombatUnit target)
    {
        List<CombatUnit> allEnemyReferences = GetAllEnemyReferences();
        var indexOfTarget = allEnemyReferences.FindIndex(position => position == (target));
        List<CombatUnit> targetAndActiveAdjacentTargets = new List<CombatUnit> {target};
        
        if (indexOfTarget < 3)
        {
            if (shouldAddToList(allEnemyReferences[indexOfTarget + 1]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget + 1]);
            } else if (shouldAddToList(allEnemyReferences[indexOfTarget + 2]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget + 2]);
            }
            
        }
        else
        {
            if (shouldAddToList(allEnemyReferences[indexOfTarget - 1]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget - 1]);
            }
            else if (shouldAddToList(allEnemyReferences[indexOfTarget - 2]))
            {
                targetAndActiveAdjacentTargets.Add(allEnemyReferences[indexOfTarget - 2]);
            }  
        }
        
        return targetAndActiveAdjacentTargets;
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
        player.GetComponent<CombatEffectManager>().ProcessActiveEffects(true);
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
        enemy.GetComponent<CombatEffectManager>().ProcessActiveEffects(true);
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
    
    /*
     * Assist methods
     */
    private List<CombatUnit> GetAllEnemyReferences()
    {
        List<CombatUnit> allPositions = new List<CombatUnit>();

        allPositions.Add(topEnemy);
        allPositions.Add(frontTopEnemy);
        allPositions.Add(centerEnemy);
        allPositions.Add(frontBottomEnemy);
        allPositions.Add(bottomEnemy);
        return allPositions;
    }

    private bool shouldAddToList(CombatUnit enemy)
    {
        return enemy && enemy.isActiveAndEnabled;
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
        
        yield return new WaitForSeconds(playerCombatAnimationSpeed); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'

        results?.ForEach(CheckForDeath);
        
        player.CombatEffectsManager.ProcessActiveEffects(false);
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

        yield return new WaitForSeconds(playerCombatAnimationSpeed); // TODO Decide how long moves should take - dynamic, static or variable. Dont hardcode '2'
        
        results?.ForEach(CheckForDeath);
        
        player.CombatEffectsManager.ProcessActiveEffects(false);
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
        
        enemy.CombatEffectsManager.ProcessActiveEffects(false);
        CheckForDeath(result);
        SetNextState();
    }

    private IEnumerator VictorySequence()
    {
        combatLog.PlayerWon();

        FindObjectOfType<GameManager>().UpdatePlayerDataAfterCombat(new CombatResult(10, player.CurrentHp));
        FindObjectOfType<GameEvents>().CombatVictoryInvoke();
        
        yield return new WaitForSeconds(2f);

        FindObjectOfType<SceneTransition>().LoadScene(SceneIndexType.Exploration);
    }
    
    // Properties
    public CombatState State
    {
        get => state;
        set => state = value;
    }

    public int RemainingPlayerActions
    {
        get => remainingPlayerActions;
        set => remainingPlayerActions = value;
    }

    public CombatUnit Player
    {
        get => player;
    }
    
}
