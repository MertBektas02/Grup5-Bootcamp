using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;

    public void UpdateAmmo(int currentAmmo, int reserveAmmo)
    {
        if (ammoText)
            ammoText.text = currentAmmo.ToString();
    
        if (totalAmmoText)
            totalAmmoText.text = reserveAmmo.ToString();
    }
}