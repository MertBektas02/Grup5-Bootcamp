using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadarSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Obje aktif olduğunda sesi başlat
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void OnDisable()
    {
        // Obje devre dışı kalınca sesi durdur
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}