using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CurrentResourceUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI woodAmount;
    public GameObject InfoPanel;
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
            InfoPanel.SetActive(!InfoPanel.activeSelf); 
        }
    }
    public void UpdateUI()
    {
        woodAmount.text = "Wood: " + ResourceManager.Instance.GetResourceAmount(ResourceType.Wood);
    }
}
