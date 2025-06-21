using System.Collections.Generic;
using UnityEngine;

public class UpgradeTool : MonoBehaviour
{
    public List<ResourceCost> cost;

    public void TryUpgrade()
    {
        if (ResourceManager.Instance.TrySpendResources(cost))
        {
            Debug.Log("Upgrade başarılı!");

        }
        else
        {
            Debug.Log("Yeterli kaynak yok!");
        }
    }
}
