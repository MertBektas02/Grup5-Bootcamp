using UnityEngine;
using UnityEngine.UI;
public class NewPlayerStatsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public Image hungerBar;
    public Image thirstBar;
    [Header("Player Stats")]
    [Range(0, 100)] public float health = 100f;
    [Range(0, 100)] public float hunger = 100f;
    [Range(0, 100)] public float thirst = 100f;
    private float maxValue = 100f;
    void Update()
    {
        // UI'daki fillAmount değerlerini güncelle
        healthBar.fillAmount = health / maxValue;
        hungerBar.fillAmount = hunger / maxValue;
        thirstBar.fillAmount = thirst / maxValue;
        // Test: Zamanla açlık/susuzluk azalsın
        hunger -= Time.deltaTime * 1f;
        thirst -= Time.deltaTime * 1.5f;
        // Can azalması test için:
        if (Input.GetKeyDown(KeyCode.H))
            health -= 10f;
        ClampStats();
    }
    void ClampStats()
    {
        health = Mathf.Clamp(health, 0, maxValue);
        hunger = Mathf.Clamp(hunger, 0, maxValue);
        thirst = Mathf.Clamp(thirst, 0, maxValue);
    }
}