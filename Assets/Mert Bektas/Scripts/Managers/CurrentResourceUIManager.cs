using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentResourceUIManager : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("UI References")]
    
    public GameObject InfoPanel;
    public GameObject crossHairCanvas;
    public MonoBehaviour playerMovementScript;
    public TextMeshProUGUI currentHealthText;
    [Header("Player current stats")]
    public TextMeshProUGUI currentFood;
    public TextMeshProUGUI currentWater;
    [Header("Player Storage UI ")]
    public TextMeshProUGUI woodAmount;
    public TextMeshProUGUI foodAmount;
    public TextMeshProUGUI waterAmount;
    // public TreeData currentData; //just in case if i needed;
    // public void ShowCurrentResources(TreeData data)//just in case if i needed;
    // {
    //     currentData = data;


    // }


    void Start()
    {
        UpdateUI();
    }
       void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UpdateUI();
            bool isPanelActive = !InfoPanel.activeSelf;
            InfoPanel.SetActive(isPanelActive);

            // Cursor'ı kontrol et
            if (isPanelActive)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
                if (playerMovementScript != null)
                    playerMovementScript.enabled = false;
                
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                if (playerMovementScript != null)
                    playerMovementScript.enabled = true;
            }
        }
    }
    public void UpdateUI()
    {
        currentHealthText.text = "health: " + player.currentHealth;
        //player current stats
        currentFood.text = "food: " + player.currentFood;
        currentWater.text = "water: " + player.currentWater;

        //player storage
        woodAmount.text = "Wood: " + ResourceManager.Instance.GetResourceAmount(ResourceType.Wood);
        foodAmount.text = "food: " + ResourceManager.Instance.GetResourceAmount(ResourceType.Food);
        waterAmount.text = "water: " + ResourceManager.Instance.GetResourceAmount(ResourceType.Water);

    }
}
