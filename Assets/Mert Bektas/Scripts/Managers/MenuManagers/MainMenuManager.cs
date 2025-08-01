using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void StartGame()
    {
        var dataManager = FindFirstObjectByType<DataPersistenceManager>();
        if (dataManager != null)
        {
            dataManager.ResetSaveData();
            // dataManager.NewGame();
            // dataManager.SaveGame();
        }
        GameOverManager.Instance.ResumeGame();
        SceneManager.LoadScene(sceneName: "StoryMenu");
    }
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnClick_LoadLastSave()
    {
        DataPersistenceManager.Instance.LoadLastSaveGame();
        Debug.Log("load butonuna tıklanıldı");
    }
}
