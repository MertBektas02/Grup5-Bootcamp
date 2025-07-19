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
    public int reserveAmmo = 6; 
    public float reloadTime = 2f;

    [Header("Effects")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyClickSound;
    private AudioSource audioSource;
    
    [Header("UI")]
    public AmmoUI ammoUI;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentAmmo = maxAmmo;
        
       //*GameObject handObj = GameObject.FindWithTag("PlayerHand");
        //*playerHand = handObj.transform;
        
    }

    private void Start()
    {
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);
    }

    private void Update()
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
            else if (Input.GetButtonDown("Fire1") && currentAmmo <= 0 && !isReloading)
            {
                audioSource.PlayOneShot(emptyClickSound);
            }

            // Yeniden doldurma
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    
    private void Shoot()
    {
        if (isReloading || currentAmmo <= 0) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        
        bulletRb.isKinematic = false;
        bulletRb.AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);
        
        // Raycast ile anlık hasar kontrolü
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position,fpsCamera.transform.forward, out hit, shootRange))
        {
            var zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage((int)damage);
            }
            
        }
        PlayMuzzleFlash();
        currentAmmo--;
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);
        audioSource.PlayOneShot(shootSound);

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
        
    }


    private IEnumerator Reload()
    {
        isReloading = true;
        audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;
        
        isReloading = false;
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);
    }

    private void PlayMuzzleFlash()
    {
        GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Destroy(flash, 0.1f);
        
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
        
        rb.detectCollisions = false;
        SetColliderEnabled(false);
    }

    public void DropFromHand()
    {
        isEquipped = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.detectCollisions = true; // ← geri aç
        SetColliderEnabled(true);
        transform.position = playerHand.position + playerHand.forward * 1f;
        transform.eulerAngles += new Vector3(0, 0, -45);
    }
    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);
    }
    void SetColliderEnabled(bool isEnabled)
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = isEnabled;
    }
}