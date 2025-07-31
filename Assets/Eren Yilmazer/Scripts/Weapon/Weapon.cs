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
    public float bulletForce = 500f;
    
    [Header("Bullet Hole")]
    public GameObject bulletHolePrefab;

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
            
            if (isReloading)
                return;

            
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
            else if (Input.GetButtonDown("Fire1") && currentAmmo <= 0 && !isReloading)
            {
                audioSource.PlayOneShot(emptyClickSound);
            }

            
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }
    // Kamera sarsılma çözüm
    void LateUpdate()
    {
        if (isEquipped)
        {
            Vector3 offset = fpsCamera.transform.right * 0.3f + fpsCamera.transform.up * -0.3f + fpsCamera.transform.forward * 0.5f;
            transform.position = fpsCamera.transform.position + offset;
            transform.rotation = fpsCamera.transform.rotation;
        }
    }

    
    private void Shoot()
    {
        if (isReloading || currentAmmo <= 0) return;

        // Mermi çıkış efekti ve sesi
        PlayMuzzleFlash();
        audioSource.PlayOneShot(shootSound);
    
        // Mermi miktarını güncelle
        currentAmmo--;
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);

        // Raycast ile hedef kontrolü
        Ray ray = fpsCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
    
        // Mermi fiziksel olarak fırlatma
        Vector3 direction = ray.direction;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletForce, ForceMode.Impulse);

        // Raycast ile anlık hasar kontrolü (SADECE BİR ŞEYE ÇARPTIYSA)
        if (Physics.Raycast(ray, out hit, shootRange))
        {
            // Düşman vurulduysa
            var mousey = hit.collider.GetComponentInParent<MouseyAI>();
            if (mousey)
            {
                mousey.TakeDamage((int)damage);
            }
            // Duvar/diğer yüzeyler için delik efekti
            else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            {
                var holeRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                var holePosition = hit.point + hit.normal * 0.01f;
                var hole = Instantiate(bulletHolePrefab, holePosition, holeRotation);
                Destroy(hole, 2f);
            }
        }

        // Şarjör kontrolü
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
        
        
    }

    public void DropFromHand()
    {
        isEquipped = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.detectCollisions = true; 
        transform.position = playerHand.position + playerHand.forward * 1f;
        transform.eulerAngles += new Vector3(0, 0, -45);
        
    }
    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
        ammoUI.UpdateAmmo(currentAmmo, reserveAmmo);
    }
   
    
}