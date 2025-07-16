using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject crossHairImage;

    [Header("Player Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;

    public static GameOverManager Instance { get; private set; }

    public bool IsGameOver { get; private set; } = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); return;

        }
        Instance = this;
    }

    public void ShowGameOver()
    {
        if (IsGameOver) return;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (crossHairImage != null)
        {
            crossHairImage.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(sceneName: "MainMenuMain");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
        public void ResumeGame() 
    {
        Time.timeScale = 1f;
        IsGameOver = false;
    }
}
