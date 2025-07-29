using System.Collections.Generic;
using UnityEngine;

public class PurchasableObject : MonoBehaviour, IDataPersistence
{
    [Header("Satın Alma Ayarları")]
    [SerializeField] private string uniqueID;
    [SerializeField] private List<ResourceCost> costList;
    [SerializeField] private GameObject targetObject; // Satın alındığında aktif olacak nesne

    private bool isPurchased = false;

    private void Start()
    {
        if (isPurchased)
            ApplyPurchaseEffect();
    }

    public void AttemptPurchase()
    {
        Debug.Log($"[{uniqueID}] Satın alma denemesi başladı. isPurchased = {isPurchased}");
        NotificationManager.Instance.ShowNotification("Purchased!", 2f, SoundType.Purchased);


        if (isPurchased)
        {
            Debug.Log($"[{uniqueID}] zaten satın alınmış.");
            return;
        }

        if (ResourceManager.Instance.TrySpendResources(costList))
        {
            Debug.Log($"[{uniqueID}] başarıyla satın alındı.");
            isPurchased = true;
            ApplyPurchaseEffect();
            SoundManager.PlaySound(SoundType.Purchased);
        }
        else
        {
            NotificationManager.Instance.ShowNotification("Not enough Resources!", 2f, SoundType.Denied);
            SoundManager.PlaySound(SoundType.Denied);
        }
    }

    private void ApplyPurchaseEffect()
    {
        if (targetObject != null)
            targetObject.SetActive(true);
    }


    // Save/Load 
    public void LoadData(GameData data)
    {
        foreach (var entry in data.purchasedObjects)
        {
            if (entry.id == uniqueID)
            {
                isPurchased = entry.isPurchased;
                if (isPurchased)
                    ApplyPurchaseEffect();
                break;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        bool found = false;
        foreach (var entry in data.purchasedObjects)
        {
            if (entry.id == uniqueID)
            {
                entry.isPurchased = isPurchased;
                found = true;
                break;
            }
        }

        if (!found)
        {
            data.purchasedObjects.Add(new PurchasedObjectEntry
            {
                id = uniqueID,
                isPurchased = isPurchased
            });
        }
    }
}
