
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Button upgradeButton;
    public Text upgradeDescription;

    public void Upgrade()
    {
        Debug.Log("Upgrade triggered.");
        upgradeDescription.text = "Upgraded!";
    }
}
