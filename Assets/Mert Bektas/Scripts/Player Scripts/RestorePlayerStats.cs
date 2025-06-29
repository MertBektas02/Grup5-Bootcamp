using UnityEngine;

public class RestorePlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CurrentResourceUIManager manager;
    private int getCurrentFood;
    private int getCurrentWater;

    public void EatFood(int amount)
    {
        if (ResourceManager.Instance.UseResource(ResourceType.Food, amount))
        {
            player.currentFood = Mathf.Min(player.currentFood + amount, 100); // max 100 olsun
            Debug.Log($"Food yendi. currentFood: {player.currentFood}");
        }
    }

    public void DrinkWater()
    {
        int maxWater = 100;
        int sipAmount = 10; // her içmede alınacak su miktarı
        int neededWater = maxWater - player.currentWater;

        if (neededWater <= 0)
        {
            Debug.Log("Zaten maksimum suya sahipsin.");
            return;
        }

        int availableWater = ResourceManager.Instance.GetResourceAmount(ResourceType.Water);
        int actualDrinkAmount = Mathf.Min(sipAmount, neededWater, availableWater);

        if (actualDrinkAmount > 0)
        {
            bool success = ResourceManager.Instance.UseResource(ResourceType.Water, actualDrinkAmount);
            if (success)
            {
                player.currentWater += actualDrinkAmount;
                manager.UpdateUI();
                SoundManager.PlaySound(SoundType.DrinkWater);
                Debug.Log($"Su içildi. +{actualDrinkAmount} currentWater: {player.currentWater}");
            }
        }
        else
        {
            Debug.Log("Yeterli su kaynağın yok.");
        }
    }

    public void EatFood()
    {
        int maxFood = 100;
        int biteAmount = 10; // her yeme işleminde alınacak gıda miktarı
        int neededFood = maxFood - player.currentFood;

        if (neededFood <= 0)
        {
            Debug.Log("Zaten tok durumdasın (maksimum food).");
            return;
        }

        int availableFood = ResourceManager.Instance.GetResourceAmount(ResourceType.Food);
        int actualEatAmount = Mathf.Min(biteAmount, neededFood, availableFood);

        if (actualEatAmount > 0)
        {
            bool success = ResourceManager.Instance.UseResource(ResourceType.Food, actualEatAmount);
            if (success)
            {
                player.currentFood += actualEatAmount;
                manager.UpdateUI();
                SoundManager.PlaySound(SoundType.EatFood);
                Debug.Log($"Yemek yendi. +{actualEatAmount} currentFood: {player.currentFood}");
            }
        }
        else
        {
            Debug.Log("Yeterli gıda kaynağın yok.");
        }
    }
}
