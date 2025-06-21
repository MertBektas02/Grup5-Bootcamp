using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Wood,
    Stone,
    Iron,
    Food,
    Fiber
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private Dictionary<ResourceType, int> resourceAmounts = new Dictionary<ResourceType, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeResources()
    {
        foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmounts[resource] = 0;
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (resourceAmounts.ContainsKey(type))
        {
            resourceAmounts[type] += amount;
            Debug.Log(type + " miktarı: " + resourceAmounts[type]);
            // Burada UI güncelleme çağrısı yapılabilir
        }
    }

    public bool UseResource(ResourceType type, int amount)
    {
        if (resourceAmounts.ContainsKey(type) && resourceAmounts[type] >= amount)
        {
            resourceAmounts[type] -= amount;
            Debug.Log(type + " kullanıldı, kalan: " + resourceAmounts[type]);
            // Burada UI güncelleme çağrısı yapılabilir
            return true;
        }
        else
        {
            Debug.Log("Yeterli " + type + " yok!");
            return false;
        }
    }

    public int GetResourceAmount(ResourceType type)
    {
        if (resourceAmounts.ContainsKey(type))
            return resourceAmounts[type];
        return 0;
    }


    //UPGRADES//
    public bool TrySpendResources(List<ResourceCost> costList)
{
    // 1. Önce hepsi var mı diye kontrol et
    foreach (var cost in costList)
    {
        if (GetResourceAmount(cost.type) < cost.amount)
        {
            Debug.Log("Yeterli " + cost.type + " yok!");
            return false;
        }
    }

    // 2. Hepsi varsa şimdi harcayalım
    foreach (var cost in costList)
    {
        UseResource(cost.type, cost.amount);
    }

    return true;
}
}
