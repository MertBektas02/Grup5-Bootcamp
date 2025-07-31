using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask ammoLayer;
    public float pickupCooldown = 0.5f; // Yeni eklendi
    
    private Camera playerCamera;
    private float lastPickupTime; // Yeni eklendi

    private void Start()
    {
        playerCamera = Camera.main;
        lastPickupTime = -pickupCooldown; // Başlangıçta hemen toplamaya izin ver
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastPickupTime + pickupCooldown)
        {
            lastPickupTime = Time.time;
            TryPickupAmmo();
        }
    }

    void TryPickupAmmo()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, ammoLayer))
        {
            AmmoBox ammoBox = hit.collider.GetComponent<AmmoBox>();
            if (ammoBox)
            {
                Weapon weapon = GetComponentInChildren<Weapon>();
                if (weapon)
                {
                    weapon.AddAmmo(ammoBox.ammoAmount);
                    ammoBox.PlayPickupSoundAndDestroy();
                }
            }
        }
    }
}