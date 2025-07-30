using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void StartGame()
    {
        GameOverManager.Instance.ResumeGame();
        SceneManager.LoadScene(sceneName: "TutorialScene");
    }
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadWithButtonMenu()
    {
        DataPersistenceManager.Instance.LoadGame();

    }
}
