using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                new SerializableDictionary<string, bool>(),
                playerUnitBase
            ),
            new SettingsData(),
            new SkillData(),
            new LevelUpData(),
            new CombatEncounterData()
        );
        
        // Test save
        gameData.playerData.chestsCollected.Add("chest1", true);
        
        gameManager.LoadData(gameData);
    }
    
    public void LoadGame()
    {
        gameData = fileHandler.Load();
        
        if (this.gameData == null)
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
        persistenceObjects.ForEach(obj => obj.SaveData(gameData));
        fileHandler.Save(gameData);
    }

    // Saves game on exit as default.
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    
}
