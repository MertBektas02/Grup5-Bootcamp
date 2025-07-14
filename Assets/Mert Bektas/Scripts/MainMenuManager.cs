using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName: "MBScene");
    }
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
