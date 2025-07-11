using UnityEngine;

public class DeskJobs : MonoBehaviour
{
    [Header("UI")]
    public GameObject openPcPanel;
    public GameObject pressF;

    private bool isPlayerInTrigger = false;

    public PcUIManager pcUIManager;

    [Header("UI Cursor")]
    [SerializeField] private GameObject customCursorImage;


    void Update()
    {
        ComputerInteraction();
    }
    public void ShowPopUp(bool show)
    {
        if (pressF != null)
            pressF.SetActive(show);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPopUp(true);
            isPlayerInTrigger = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPopUp(false);
            isPlayerInTrigger = false;
        }
    }
    void ComputerInteraction()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            pcUIManager.TogglePcMainPanel();
        }
    }
    void TogglePcPanel()
    {
        if (openPcPanel != null)
        {
            openPcPanel.SetActive(!openPcPanel.activeSelf);
        }
    }
}
