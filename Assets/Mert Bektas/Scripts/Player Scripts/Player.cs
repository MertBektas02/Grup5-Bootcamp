using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour, IDataPersistence
{
    public PlayerData data;
    [Header("current stats")]
    public int currentHealth = 100;
    public int currentFood = 100;
    public int currentWater = 100;

    private float damageTimer = 0f;
    private float healingTimer = 0f;
    private const float damageInterval = 5f;


    void Start()
    {
        InvokeRepeating(nameof(ReduceNeeds), 0f, 5f); // 5 saniyede bir
    }


    void Update()
    {
        if (currentHealth == 0) return;
        {
            if (currentFood == 0 || currentWater == 0)
            {
                healingTimer += Time.deltaTime;

                if (healingTimer >= damageInterval)
                {
                    TakeDamage(5);
                    healingTimer = 0f;
                }
            }
            else
            {
                healingTimer = 0f;
            }
        }


        //healing
        if (currentFood >= 85 && currentFood == 100 || currentWater >= 85 && currentWater == 100)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                RecoveryHealth(5);
                damageTimer = 0f;
            }
        }
        else
        {
            damageTimer = 0f;
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage. Current health: " + currentHealth);
    }
    public void RecoveryHealth(int amount) // food ve water 85'in üstündeyse can yenilensin.
    {
        currentHealth += amount;
        Debug.Log("Player healing. Current health: " + currentHealth);
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
