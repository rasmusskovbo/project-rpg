using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
 * Change filehandler to interface (facade)
 */
public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
{
    [Header("New Game Settings")]
    [SerializeField] private UnitBase playerUnitBase;

    [Header("Persistence configuration")] 
    [SerializeField] private string fileName;

    private List<IDataPersistence> persistenceObjects;
    private GameData gameData;
    private GameManager gameManager;
    private FileHandler fileHandler;

    private void Start()
    {
        this.persistenceObjects = FindAllDataPersistenceObjects();
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        gameManager = FindObjectOfType<GameManager>();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData(
            new PlayerData(
                1,
                0,
                10,
                0,
                0,
                new Vector3(-12.5f, 3.75f),
                PlayerFacing.South,
                playerUnitBase,
                playerUnitBase.MaxHp
            ),
            new SettingsData(),
            new SkillData(),
            new LevelUpData(),
            new InventoryData(new SerializableDictionary<InventoryItem, int>()),
            new EquipmentData(null, null, null,null,null,null,null),
            new CombatEncounterData(),
            new NPCData(new SerializableDictionary<string, bool>()),
            new QuestData(
                new List<Quest>(),
                new SerializableDictionary<QuestGoal, int>()
                )
        );
        
        // Test save
        //gameData.playerData.chestsCollected.Add("chest1", true);
        
        gameManager.LoadData(gameData);
    }
    
    public void LoadGame()
    {
        gameData = fileHandler.Load();
        bool isNewGame = false;
        
        if (FindObjectOfType<MainMenuController>() != null)
        {
            isNewGame = FindObjectOfType<MainMenuController>().IsNewGame();  
        }
        
        if (this.gameData == null || isNewGame)
        {
            Debug.Log("No data found. Initializing to default");
            NewGame();
        }
        else
        {
            persistenceObjects.ForEach(obj => obj.LoadData(gameData));
            Debug.Log("Loaded data successfully.");
        }
        
    }

    public void SaveGame()
    {
        FindAllDataPersistenceObjects().ForEach(obj => obj.SaveData(gameData));
        fileHandler.Save(gameData);
        Debug.Log("Game saved");
    }

    /*
     private void OnApplicationQuit()
    {
        SaveGame();
    } 
     */
    
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    
}
