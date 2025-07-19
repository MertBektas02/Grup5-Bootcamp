using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FlashBomb : MonoBehaviour
{
    [Header("Flash Bomb Ayarları")]
    public Transform playerHand;
    public Camera fpsCamera;
    public float throwForce = 10f;
    
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
        GameObject handObj = GameObject.FindWithTag("PlayerHand");
        playerHand = handObj.transform;
    }

    void Update()
    {
        if (hasBeenThrown) return;

        if (isEquipped && Input.GetButtonDown("Fire1"))
        {
            Throw();
        }
    }

    
    void Throw()
    {
        isEquipped = false;
        hasBeenThrown = true;
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 throwDirection = fpsCamera.transform.forward + fpsCamera.transform.up * 0.5f;
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);
        
        audioSource.PlayOneShot(explosionSound);
        
        // FlashBomb ikonunu kapat
        EquipmentManager.Instance.flashBombIcon.gameObject.SetActive(false);
        

        StartCoroutine(DelayedExplosion());
    }

    
    private IEnumerator DelayedExplosion()
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
        EquipmentManager.Instance.ClearFlashBomb(); 
        Destroy(gameObject, 5f); // bombayı sonra yok et
    }


    
    public bool IsEquipped() => isEquipped;

    public void SetEquipped(bool val)
    {
        isEquipped = val;
        
    }

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