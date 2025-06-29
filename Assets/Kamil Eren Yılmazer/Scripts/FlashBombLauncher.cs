using UnityEngine;

public class FlashBombLauncher : MonoBehaviour
{
    public GameObject flashBombPrefab;
    public Transform throwOrigin;
    public float throwForce = 10f;
    private bool isEquipped = false;
    public GameObject FlashBombPickup;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isEquipped = !isEquipped;
            FlashBombPickup.SetActive(true);
        }

        if (isEquipped && Input.GetButtonDown("Fire1"))
        {
            ThrowFlashBomb();
            FlashBombPickup.SetActive(false);
        }
    }

    void ThrowFlashBomb()
    {
        GameObject bomb = Instantiate(flashBombPrefab, throwOrigin.position, throwOrigin.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        Vector3 throwDirection = throwOrigin.forward + throwOrigin.up * 0.5f;
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);
        isEquipped = false; // tek kullanımlık
    }
}