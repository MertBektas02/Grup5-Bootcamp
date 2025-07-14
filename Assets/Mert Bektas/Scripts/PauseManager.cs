using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject crosshair;

    [Header("Player")]
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    private MouseLook mouseLook;

    private bool isPaused = false;
    private bool cursorWasLockedBeforePause;

    void Start()
    {
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            mouseLook = player.GetComponentInChildren<MouseLook>();
        }

        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        if (crosshair != null) crosshair.SetActive(false);
        Time.timeScale = 0f;

        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        cursorWasLockedBeforePause = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        if (crosshair != null) crosshair.SetActive(true);
        Time.timeScale = 1f;

        if (playerMovement != null) playerMovement.enabled = true;
        if (mouseLook != null) mouseLook.enabled = true;

        if (cursorWasLockedBeforePause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
