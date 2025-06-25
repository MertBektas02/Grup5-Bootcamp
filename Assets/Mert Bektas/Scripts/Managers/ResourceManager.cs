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
public class ResourceManager : MonoBehaviour,IDataPersistence
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
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmounts[type] = 0;
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

    //save sistemi için kaynakların hepsini tek seferde alabileceğimiz bir getter metodu
    public Dictionary<ResourceType, int> GetAllResources()
    {
        return new Dictionary<ResourceType, int>(resourceAmounts);
    }
    // load sırasında kaynakları sıfırdan setleyeceğimiz için bir setter metodu
    public void SetResourceAmount(ResourceType type, int amount)
    {
        if (resourceAmounts.ContainsKey(type))
            resourceAmounts[type] = amount;
        else
            resourceAmounts.Add(type, amount);
    }

    // ------------------- IDataPersistence -------------------

    public void LoadData(GameData data)
    {
        if (data.resourceAmounts == null || data.resourceAmounts.Count == 0)
            return;

        foreach (var res in data.resourceAmounts)
        {
            SetResourceAmount(res.type, res.amount);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.resourceAmounts.Clear();

        foreach (var kvp in resourceAmounts)
        {
            data.resourceAmounts.Add(new ResourceAmount
            {
                type = kvp.Key,
                amount = kvp.Value
            });
        }
    }

    //UPGRADES//
    public bool TrySpendResources(List<ResourceCost> costList)
    {

        foreach (var cost in costList)
        {
            if (GetResourceAmount(cost.type) < cost.amount)
            {
                Debug.Log("Yeterli " + cost.type + " yok!");
                return false;
            }
        }

        foreach (var cost in costList)
        {
            UseResource(cost.type, cost.amount);
        }

        return true;
    }
}
