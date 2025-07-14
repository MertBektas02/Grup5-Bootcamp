using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject Panel;

    [Header("Settings UI")]
    public Slider volumeSlider;
    public Toggle musicToggle;

    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (Panel != null) Panel.SetActive(true);

    

        if (musicToggle != null)
            musicToggle.onValueChanged.AddListener(ToggleMusic);
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame fonksiyonu �a�r�ld�");
        SceneManager.LoadScene("TutorialScene");
    }
    public void OpenSettings()
    {
        Debug.Log("Settings butonuna bas�ld�");
            settingsPanel.SetActive(true);
            Panel.SetActive(false);
    }
    public void CloseSettings()
    {
        Debug.Log("Settings kapat�ld�");

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (Panel != null)
            Panel.SetActive(true);
    }
    public void SetVolume(float value)
    {
        Debug.Log("Volume ayarland�: " + value);
    }
    public void ToggleMusic(bool isOn)
    {
        Debug.Log("Music toggle: " + isOn);
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan ��k�l�yor...");
        Application.Quit();
    }
}
