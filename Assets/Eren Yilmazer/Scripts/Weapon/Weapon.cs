using UnityEngine;
using System.Collections;
using TMPro;
public class Weapon : MonoBehaviour
{
    [Header("Silah Ayarları")]
    public Transform playerHand;
    public Camera fpsCamera;
    public float damage = 25f;
    public float shootRange = 50f;
    
    [Header("Atış Ayarları")]
    public float fireRate = 0.5f; // Saniyede 2 atış
    private float nextFireTime = 0f;
    
    private Rigidbody rb;
    private bool isEquipped = false;
    private bool isReloading = false;
    
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletForce = 50f;

    [Header("Muzzle Flash")]
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePoint;
    
    [Header("Ammo")]
    public int maxAmmo = 6;
    private int currentAmmo;
    public float reloadTime = 2f;

    [Header("Effects")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    private AudioSource audioSource;
    
    [Header("UI")]
    public AmmoUI ammoUI;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        fpsCamera = Camera.main;
        currentAmmo = maxAmmo;
        
        GameObject handObj = GameObject.FindWithTag("PlayerHand");
        playerHand = handObj.transform;
        
    }
    void Start()
    {
            ammoUI.UpdateAmmo(currentAmmo, maxAmmo);
    }
    void Update()
    {
        if (isEquipped)
        {
            // Yeniden doldurma kontrolü
            if (isReloading)
                return;

            // Atış kontrolü
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
            

            // Yeniden doldurma
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        if (isReloading || currentAmmo <= 0) return;
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            
            if (bulletRb != null)
            {
                bulletRb.isKinematic = false;
                bulletRb.AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);
            }
        }

        // Raycast ile anlık hasar kontrolü
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, shootRange))
        {
            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage((int)damage);
            }
        }

        // Efektler
        PlayMuzzleFlash();
        currentAmmo--;
        
        if (ammoUI != null)
            ammoUI.UpdateAmmo(currentAmmo, maxAmmo);

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
        
    }


    private IEnumerator Reload()
    {
        isReloading = true;
        audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        if (ammoUI != null)
            ammoUI.UpdateAmmo(currentAmmo, maxAmmo);
    }

    void PlayMuzzleFlash()
    {
        if (muzzleFlashPrefab != null && muzzlePoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Destroy(flash, 0.1f);
        }
    }

    // ----------- Yeni Fonksiyonlar -----------
    public bool IsEquipped() => isEquipped;

    public void SetEquipped(bool val) => isEquipped = val;

    public void MoveTo(Transform target)
    {
        isEquipped = false;
        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
    }

    public void DropFromHand()
    {
        isEquipped = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        transform.position = playerHand.position + playerHand.forward * 1f;
        transform.eulerAngles += new Vector3(0, 0, -45);
    }
    
}