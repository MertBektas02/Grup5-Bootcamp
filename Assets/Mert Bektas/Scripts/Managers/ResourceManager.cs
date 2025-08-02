using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Wood,
    Stone,
    Iron,
    Food,
    Fiber,
    Water,
    Coal
}
public class ResourceManager : MonoBehaviour, IDataPersistence
{
    public static ResourceManager Instance;

    private Dictionary<ResourceType, int> resourceAmounts = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> maxResourceAmounts = new Dictionary<ResourceType, int>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeResources();
            InitializeMaxValues();
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



    //--------initialization-------.

    private void InitializeResources()
    {
        foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceAmounts[resource] = 0;
        }
    }
    private void InitializeMaxValues()//depo
    {
        maxResourceAmounts[ResourceType.Wood] = 400;
        maxResourceAmounts[ResourceType.Stone] = 400;
        maxResourceAmounts[ResourceType.Iron] = 400;
        maxResourceAmounts[ResourceType.Food] = 1000;
        maxResourceAmounts[ResourceType.Fiber] = 400;
        maxResourceAmounts[ResourceType.Water] = 1000;
        maxResourceAmounts[ResourceType.Coal] = 400;
    }
    //--------initialization-------.




    public void AddResource(ResourceType type, int amount)
    {
        if (!resourceAmounts.ContainsKey(type)) return;

        int current = resourceAmounts[type];
        int max = GetMaxResourceAmount(type); // maxResourceAmounts'tan al

        int newAmount = current + amount;
        if (newAmount > max)
        {
            newAmount = max; // fazla eklemeyi kırp
        }

        resourceAmounts[type] = newAmount;
        //Debug.Log(type + " miktarı: " + resourceAmounts[type] + " / " + max);
        // Burada UI güncelleme çağrısı yapılabilir
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
    public bool TrySpendResources(List<ResourceCost> costList)//upgrade'ler için kullanmayı planlıyorum.
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

    public int GetResourceAmount(ResourceType type)
    {
        if (resourceAmounts.ContainsKey(type))
            return resourceAmounts[type];
        return 0;
    }
    public int GetMaxResourceAmount(ResourceType type)//UI'da ayrı bir update'e ihtiyaç duyarsam bunu kullanırım.
    {
        if (maxResourceAmounts.ContainsKey(type))
            return maxResourceAmounts[type];
        return int.MaxValue;
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

    // ------------------- Save/Load -------------------

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

}
