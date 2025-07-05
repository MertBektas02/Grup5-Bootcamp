using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject weaponModel;         // Elimizdeki silah (örnek: pistol objesi)
    public Camera fpsCamera;
    public float damage = 25f;
    public float range = 50f;
    
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponModel.SetActive(!weaponModel.activeSelf);
        }

        if (weaponModel.activeSelf && Input.GetButtonDown("Fire1"))
        {
            Shoot();
            audioSource.PlayOneShot(this.audioSource.clip);
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.TakeDamage((int)damage);
            }
        }
    }
}