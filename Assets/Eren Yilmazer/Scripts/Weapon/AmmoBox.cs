using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 1;
    private AudioSource audioSource;
    public AudioClip pickupSound;
    private bool isPickedUp = false;
    private Collider boxCollider; // Yeni eklendi

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<Collider>(); // Yeni eklendi
    }

    public void PlayPickupSoundAndDestroy()
    {
        if (isPickedUp) return;
        isPickedUp = true;

        // Collider'ı devre dışı bırak
        if (boxCollider)
            boxCollider.enabled = false;

        if (pickupSound && audioSource)
        {
            audioSource.PlayOneShot(pickupSound);
            Destroy(gameObject, pickupSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}