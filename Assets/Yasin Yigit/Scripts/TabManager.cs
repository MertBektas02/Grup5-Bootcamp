using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject[] panels;
    private int currentTabIndex = 0;

    void Start()
    {
        ShowTab(currentTabIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentTabIndex = (currentTabIndex + 1) % panels.Length;
            ShowTab(currentTabIndex);
        }
    }

    public void ShowTab(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }
}
