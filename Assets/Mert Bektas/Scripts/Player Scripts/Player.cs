using UnityEngine;

public class Player : MonoBehaviour, IDataPersistence
{
    public PlayerData data;
    public int currentHealth = 100;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage. Current health: " + currentHealth);
    }
        public void SaveData(ref GameData data)
    {
        data.playerData.playerHealth = currentHealth;
    }
        public void LoadData(GameData data)
    {
        currentHealth = data.playerData.playerHealth;
    }
}
