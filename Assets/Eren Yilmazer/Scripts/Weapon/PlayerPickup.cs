using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask ammoLayer;

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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
            if (ammoBox != null)
            {
                Weapon weapon = GetComponentInChildren<Weapon>();
                if (weapon != null)
                {
                    weapon.AddAmmo(ammoBox.ammoAmount);
                    Destroy(ammoBox.gameObject);
                }
            }
        }
    }
}