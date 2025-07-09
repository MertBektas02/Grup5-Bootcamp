using System.Collections.Generic;
using UnityEngine;

public class AutoCollectorPurchasePoint : MonoBehaviour, IDataPersistence
{
    [Header("Collector")]
    public GameObject collectorObject;

    [Header("Satın Alma Koşulları")]
    public List<ResourceCost> purchaseCosts = new List<ResourceCost>();

    [Header("UI")]
    public GameObject uiPanel;

    [SerializeField] private string uniqueID;
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
            ActivateCollector();
        }
        else
        {
            Debug.Log("Satın alma başarısız: Yetersiz kaynak.");
        }
    }

    private void ActivateCollector()
    {
        collectorObject.SetActive(true);
        collectorObject.GetComponent<AutoCollector>()?.Activate();
        isPurchased = true;

        if (uiPanel != null)
            uiPanel.SetActive(false);
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

    // ---------- SAVE / LOAD ----------
    public void SaveData(ref GameData data)
    {
        var existing = data.purchasedCollectors.Find(x => x.id == uniqueID);
        if (existing != null)
        {
            existing.isPurchased = isPurchased;
        }
        else
        {
            data.purchasedCollectors.Add(new CollectorData
            {
                id = uniqueID,
                isPurchased = isPurchased
            });
        }
    }
    public void LoadData(GameData data)
    {
        var match = data.purchasedCollectors.Find(x => x.id == uniqueID);
        if (match != null && match.isPurchased)
        {
            ActivateCollector();
        }
    }
}