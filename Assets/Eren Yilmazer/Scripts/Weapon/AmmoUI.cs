using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
            ammoText.text = currentAmmo.ToString();
        
        if (totalAmmoText != null)
            totalAmmoText.text = maxAmmo.ToString();
    }
}