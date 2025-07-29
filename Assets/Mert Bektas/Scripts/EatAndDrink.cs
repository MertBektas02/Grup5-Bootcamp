using UnityEngine;
using UnityEngine.UI;

public class EatAndDrink : MonoBehaviour
{
    [Header("Bağlantılar")]
    [SerializeField] private GameObject panelToToggle;
    [SerializeField] private PlayerMovement playerScript;
    [SerializeField] private MouseLook lookAroundScript;
    [SerializeField] private Image cursorImage;

    private bool isPanelOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel();
        }
    }

private void TogglePanel()
{
    isPanelOpen = !isPanelOpen;

    // Cursor önce ayarlanmalı
    Cursor.lockState = isPanelOpen ? CursorLockMode.None : CursorLockMode.Locked;
    Cursor.visible = isPanelOpen;

    // Panel aç/kapat
    panelToToggle.SetActive(isPanelOpen);

    // Scriptleri aktif/pasif yap
    playerScript.enabled = !isPanelOpen;
    lookAroundScript.enabled = !isPanelOpen;

    // İlgili image'i kapat
    if (cursorImage != null)
        cursorImage.gameObject.SetActive(!isPanelOpen);
}
}
