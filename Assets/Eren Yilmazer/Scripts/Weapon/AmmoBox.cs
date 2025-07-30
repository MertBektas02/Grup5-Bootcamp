using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 1;
    private AudioSource audioSource;
    public AudioClip pickupSound;

    private bool isPickedUp = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSoundAndDestroy()
    {
        if (isPickedUp) return; 
        isPickedUp = true;

        if (pickupSound && audioSource )
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