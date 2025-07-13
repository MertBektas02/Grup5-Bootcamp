using UnityEngine;
using UnityEngine.UI;

public class PcUIManager : MonoBehaviour
{
    [Header("UI Ayarları")]
    [SerializeField] private GameObject pcMainPanel;
    [SerializeField] private Button togglePcbutton;
    [SerializeField] private GameObject StoragePanel;

    [Header("Kontrol Edilecek Scriptler")]
    [SerializeField] private MouseLook mouseLookScript;
    [SerializeField] private PlayerMovement playerMovementScript;

    [Header("UI Cursor")]
    [SerializeField] private GameObject customCursorImage;

    private bool isPcOpen = false;

    private void Start()
    {
        togglePcbutton.onClick.AddListener(TogglePcMainPanel);
    }

    public void TogglePcMainPanel()
    {
        isPcOpen = !isPcOpen;
        pcMainPanel.SetActive(isPcOpen);

        mouseLookScript.enabled = !isPcOpen;
        playerMovementScript.enabled = !isPcOpen;

        if (isPcOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            customCursorImage.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            customCursorImage.SetActive(true);

        }
    }

    public void ToggleStorage()
    {
       StoragePanel.SetActive(!StoragePanel.activeSelf); 
    }
}
