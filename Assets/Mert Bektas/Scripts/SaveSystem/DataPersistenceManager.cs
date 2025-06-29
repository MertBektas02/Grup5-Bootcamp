using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool initializeNewDataIfNull = true;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, "saveData.json");
    }

    private void Start()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null && initializeNewDataIfNull)
        {
            Debug.Log("No save data found. Initializing new game data.");
            NewGame();
        }

        foreach (var obj in dataPersistenceObjects)
        {
            obj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var obj in dataPersistenceObjects)
        {
            obj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<IDataPersistence>()
            .ToList();
    }

    // Manuel olarak çağırabilmen için
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
