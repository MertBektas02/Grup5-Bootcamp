using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Ayarları")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button returnToMenuButton;

    [Header("Cursor & Crosshair")]
    [SerializeField] private GameObject crosshair;

    [Header("Kontrol Edilecek Scriptler")]
    [SerializeField] private MouseLook mouseLookScript;
    [SerializeField] private PlayerMovement playerMovementScript;

    [Header("Screen UI")]
    [SerializeField] private GameObject screenUICanvas;  // Inspector’dan ekle

    private bool isPaused = false;

    private void Start()
    {
        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(ReturnToMenu);

        // Başlangıç ayarları
        pausePanel.SetActive(false);
        if (crosshair != null) crosshair.SetActive(true);
        if (screenUICanvas != null) screenUICanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (screenUICanvas != null)
            screenUICanvas.SetActive(!isPaused);

        if (mouseLookScript != null) mouseLookScript.enabled = !isPaused;
        if (playerMovementScript != null) playerMovementScript.enabled = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (crosshair != null) crosshair.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (crosshair != null) crosshair.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuMain");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
