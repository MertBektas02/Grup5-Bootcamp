using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentResourceUIManager : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("UI References")]
    
    public GameObject InfoPanel;
    public GameObject crossHairCanvas;
    public PlayerMovement playerMovementScript;
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
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UpdateUI();
            bool isPanelActive = !InfoPanel.activeSelf;
            InfoPanel.SetActive(isPanelActive);
            crossHairCanvas.SetActive(!isPanelActive);

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
        currentHealthText.text =  player.currentHealth.ToString();

        //player current stats
        currentFood.text =  player.currentFood.ToString();
        currentWater.text =  player.currentWater.ToString();

        //player storage
        woodAmount.text =  ResourceManager.Instance.GetResourceAmount(ResourceType.Wood).ToString();
        foodAmount.text =  ResourceManager.Instance.GetResourceAmount(ResourceType.Food).ToString();
        waterAmount.text = ResourceManager.Instance.GetResourceAmount(ResourceType.Water).ToString();

    }
}
