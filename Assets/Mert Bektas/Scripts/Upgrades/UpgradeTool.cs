using System.Collections.Generic;
using UnityEngine;

public class UpgradeTool : MonoBehaviour,IDataPersistence
{
    public List<ResourceCost> cost;
    public string upgradeID;
    public bool isUnlocked;

    public void TryUpgrade()
    {
        if (isUnlocked) return;

        if (ResourceManager.Instance.TrySpendResources(cost))
        {
            isUnlocked = true;
            ApplyUpgradeEffect();
        }
        else
        {
            Debug.Log("Yeterli kaynak yok!");
        }
    }

    private void ApplyUpgradeEffect()
    {
        Debug.Log("upgrade satın alındı");
        // Stat artışı vs. buraya yazılacak
    }

    // ----------- SAVE LOAD
    public void SaveData(ref GameData data)
    {
        var existing = data.unlockedUpgrades.Find(x => x.upgradeID == upgradeID);
        if (existing != null)
        {
            existing.isUnlocked = isUnlocked;
        }
        else
        {
            data.unlockedUpgrades.Add(new UpgradeState
            {
                upgradeID = upgradeID,
                isUnlocked = isUnlocked
            });
        }
    }

    public void LoadData(GameData data)
    {
        var entry = data.unlockedUpgrades.Find(x => x.upgradeID == upgradeID);
        if (entry != null && entry.isUnlocked)
        {
            isUnlocked = true;
            ApplyUpgradeEffect();
        }
    }
    // ----------- SAVE LOAD
}
