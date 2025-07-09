using System.Collections.Generic;
using UnityEngine;

public class AutoCollectorPurchasePoint : MonoBehaviour
{
    [Header("Collector")]
    public GameObject collectorObject; // Sahnedeki toplayıcı, SetActive ile aktif edilecek

    [Header("Satın Alma Koşulları")]
    public List<ResourceCost> purchaseCosts = new List<ResourceCost>();

    [Header("UI")]
    public GameObject uiPanel;

    private bool isPurchased = false;
    private bool playerInRange = false;

    void Update()
    {
        if (isPurchased || !playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPurchase();
        }
    }

    private void TryPurchase()
    {
        if (ResourceManager.Instance.TrySpendResources(purchaseCosts))
        {
            collectorObject.SetActive(true);

            // EKLENMESİ GEREKEN SATIR:
            var collector = collectorObject.GetComponent<AutoCollector>();
            if (collector != null)
            {
                collector.Activate();
            }
            else
            {
                Debug.LogError("AutoCollector scripti atanmadı!");
            }

            isPurchased = true;
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Satın alma başarısız: Yetersiz kaynak.");
        }
    }

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
