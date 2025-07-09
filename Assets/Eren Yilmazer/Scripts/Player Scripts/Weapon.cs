using UnityEngine;

public class Weapon : MonoBehaviour
{
  [Header("Silah Ayarları")]
    public Transform playerHand;           
    public Camera fpsCamera;
    public float damage = 25f;
    public float shootRange = 50f;
    public float pickupRange = 2f;

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isEquipped = false;
    
    [Header("Muzzle Flash")]
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePoint;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // Eğer fpsCamera yoksa otomatik bul
        if (fpsCamera == null)
            fpsCamera = Camera.main;

        // Eğer playerHand yoksa sahnede tag ile bul
        if (playerHand == null)
        {
            GameObject handObj = GameObject.FindWithTag("PlayerHand");
            if (handObj != null)
                playerHand = handObj.transform;
        }
    }

    void Start()
    {
        rb.isKinematic = false; // Başlangıçta yerde olsun
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isEquipped)
            {
                float distance = Vector3.Distance(transform.position, playerHand.position);
                if (distance <= pickupRange)
                {
                    Equip();
                }
            }
            else
            {
                Unequip();
            }
        }

        if (isEquipped && Input.GetButtonDown("Fire1"))
        {
            Shoot();

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    void Equip()
    {
        isEquipped = true;
        rb.isKinematic = true;

        transform.SetParent(playerHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Unequip()
    {
        isEquipped = false;
        transform.SetParent(null);
        rb.isKinematic = false;

        // Hafif açılı yere bırak
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 45);
    }

    void Shoot()
    {
        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage((int)damage);
            }
        }
        PlayMuzzleFlash();
    }
    void PlayMuzzleFlash()
    {
        if (muzzleFlashPrefab != null && muzzlePoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Destroy(flash, 0.1f); // Efekti kısa sürede sil
        }
    }
}
