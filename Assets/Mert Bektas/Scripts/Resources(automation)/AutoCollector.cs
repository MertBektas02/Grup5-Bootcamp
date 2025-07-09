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
        }
    }

    public void Activate()
    {
        isActive = true;
        timer = 0f;
    }

    public bool IsActive => isActive;
}
