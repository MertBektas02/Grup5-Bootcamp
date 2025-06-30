using System.Collections;
using UnityEngine;

public class FlashBomb : MonoBehaviour
{
    public float explosionDelay = 5f;
    public float effectRadius = 20f;
    public float blindDuration = 5f;
    public AudioClip explosionSound;
    public GameObject explosionEffect;

    private bool hasExploded = false;
    

    void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        StartCoroutine(DelayedAffectZombies(2f));

        Destroy(gameObject,5f); // Patladıktan sonra kendini yok et
    }
    IEnumerator DelayedAffectZombies(float delay)
    {
        yield return new WaitForSeconds(delay);

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
    }

}