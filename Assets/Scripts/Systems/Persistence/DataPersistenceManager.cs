using UnityEngine;

public class DataPersistenceManager : PersistentSingleton<DataPersistenceManager>
{
    [Header("New Game Settings")]
    [SerializeField] private UnitBase playerUnitBase;

    [Header("Persistence configuration")] 
    [SerializeField] private string fileName;
    
    private GameData gameData;
    private GameManager gameManager;
    private FileHandler fileHandler;

    private void Start()
    {
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
                5,
                0,
                new Vector3(-12.5f, 3.75f),
                PlayerFacing.South,
                new SerializableDictionary<string, bool>(),
                playerUnitBase,
                playerUnitBase.MaxHp
            ),
            new SettingsData(),
            new SkillData()
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
            gameManager.LoadData(gameData);
            Debug.Log("Loaded data successfully.");
        }
        
    }

    public void SaveGame()
    {
        gameManager.SaveData(gameData);
        fileHandler.Save(gameData);
    }

    // Saves game on exit as default.
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
