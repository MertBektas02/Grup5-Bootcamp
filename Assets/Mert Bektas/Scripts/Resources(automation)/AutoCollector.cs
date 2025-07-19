using UnityEngine;
using System.Collections.Generic;
public class AutoCollector : MonoBehaviour
{
    [Header("Üretim Ayarları")]
    public ResourceType resourceType;
    public int amountPerTick = 1;
    public float tickInterval = 3f;

    private float timer;
    private bool isActive = false;
    public List<ResourceCost> purchaseCosts = new List<ResourceCost>();
    private CurrentResourceUIManager _updateUI;
    void Start()
    {
        _updateUI = FindFirstObjectByType<CurrentResourceUIManager>();

    }



    void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= tickInterval)
        {
            timer = 0f;
            ResourceManager.Instance.AddResource(resourceType, amountPerTick);
            _updateUI.UpdateUI();
            ShowAutomationPopup(amountPerTick,resourceType);
        }
    }

    public void Activate()
    {
        isActive = true;
        timer = 0f;
    }

    public bool IsActive => isActive;


    [Header("Popup Ayarları")]
    public GameObject automationPopupPrefab;     
    public Transform popupSpawnPoint;            
    private void ShowAutomationPopup(int amount, ResourceType type)
    {
        if (automationPopupPrefab == null) return;

        // Spawn pozisyonunu belirle
        Vector3 spawnPos = popupSpawnPoint != null ? popupSpawnPoint.position : transform.position;

        GameObject go = Instantiate(automationPopupPrefab, spawnPos, Quaternion.identity);

        // TextMesh’i güncelle
        TextMesh tm = go.GetComponentInChildren<TextMesh>();
        if (tm != null)
        {
            tm.text = $"+{amount} {type}";
        }
    }

}
