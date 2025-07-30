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

    //--------- MAIN MENU veya PAUSE MENU'deyken Sahne yükleme---------
    //Main menü sahnesinde son save'i yükleyebilmek için bazı değişikliklere ihtiyacım vardı.
    //LoadLastSaveGame metodu, DataTransferBetWeenScenes.cs vs.
    public void LoadLastSaveGame()
    {
        GameData loadedData = dataHandler.Load();

        if (loadedData == null)
        {
            Debug.LogWarning("No save data found.");
            return;
        }

        // Hedef sahneye geçmeden önce veri taşınsın
        if (!string.IsNullOrEmpty(loadedData.lastSceneName) &&
            loadedData.lastSceneName != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            DataTransferBetweenScenes.lastLoadedGameData = loadedData;
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedData.lastSceneName);
        }
        else
        {
            gameData = loadedData;
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame(); // aynı sahnedeysek direkt yükle
        }
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (DataTransferBetweenScenes.lastLoadedGameData != null)
        {
            gameData = DataTransferBetweenScenes.lastLoadedGameData;
            DataTransferBetweenScenes.lastLoadedGameData = null;

            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame(); // Sahne yüklendiğinde kaldığı yerden devam
        }
    }
    //--------- MAIN MENU veya PAUSE MENU'deyken Sahne yükleme---------


    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
