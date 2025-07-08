using System.Collections;
using UnityEngine;

public class FlashBomb : MonoBehaviour
{
    [Header("Flash Bomb Ayarları")]
    public Transform playerHand;
    public Camera fpsCamera;
    public float throwForce = 10f;
    public float pickupRange = 2f;
    public float explosionDelay = 2f;
    public float effectRadius = 20f;
    public float blindDuration = 5f;
    public AudioClip explosionSound;
    public GameObject explosionEffect;

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isEquipped = false;
    private bool hasBeenThrown = false;
    private bool hasExploded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (fpsCamera == null)
            fpsCamera = Camera.main;

        if (playerHand == null)
        {
            GameObject handObj = GameObject.FindWithTag("PlayerHand");
            if (handObj != null)
                playerHand = handObj.transform;
        }
    }

    void Update()
    {
        if (hasBeenThrown) return;

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
            Throw();
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

    void Unequip()
    {
        isEquipped = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 45);
    }

    void Throw()
    {
        isEquipped = false;
        hasBeenThrown = true;
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 throwDirection = fpsCamera.transform.forward + fpsCamera.transform.up * 0.5f;
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);

        StartCoroutine(DelayedExplosion());
    }

    IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (explosionSound != null && audioSource != null)
            audioSource.PlayOneShot(explosionSound);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (Collider col in hitColliders)
        {
            ZombieAI zombie = col.GetComponentInParent<ZombieAI>();
            if (zombie != null && !zombie.isDead)
            {
                zombie.activeFlashBomb = gameObject;
                zombie.BecomeBlinded(blindDuration, transform.position);
            }
        }

        Destroy(gameObject, 5f); // bombayı sonra yok et
    }
}
