using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public PlayerData data;
    [Header("current stats")]
    public int currentHealth = 100;
    public int currentFood = 100;
    public int currentWater = 100;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage. Current health: " + currentHealth);
    }
    void Start()
    {
        InvokeRepeating(nameof(ReduceNeeds), 0f, 5f); // 5 saniyede bir
    }

    void ReduceNeeds()
    {
        currentFood = Mathf.Max(currentFood - 1, 0);
        currentWater = Mathf.Max(currentWater - 1, 0);
    }
    public void SaveData(ref GameData data)
    {
        data.playerData.playerHealth = currentHealth;
        data.playerData.playerFood = currentFood;
        data.playerData.playerWater = currentWater;
    }
    public void LoadData(GameData data)
    {
        currentHealth = data.playerData.playerHealth;
        currentFood = data.playerData.playerFood;
        currentWater = data.playerData.playerWater;
    }
}
