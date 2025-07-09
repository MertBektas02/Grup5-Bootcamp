using UnityEngine;

public class UITriggerForAutoCollector : MonoBehaviour
{
    private bool playerInRange = false;
    [Header("UI")]
    public GameObject uiPanel;
        private bool isPurchased = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isPurchased && uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }
}
