using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;
    // Tek bir kaynak kullanıyoruz. İstersen 2 kaynak da yapabilirsin ama şart değil.

    [Header("Ses Listeleri")]
    public List<AudioClip> dayAmbienceClips = new List<AudioClip>();
    public List<AudioClip> nightAmbienceClips = new List<AudioClip>();

    [Header("Durum")]
    [SerializeField] private bool isNightNow = false;
    private bool isTransitioning = false;
    private Coroutine playRoutine;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
        void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.loop = false;
                audioSource.playOnAwake = false;
            }

            SetNightMode(false); // veya true yaparsan gece sesiyle başlar
        }
    }


    public void SetNightMode(bool night)
    {
        if (isNightNow == night) return; // aynı duruma tekrar geçme
        isNightNow = night;

        // devam eden coroutine varsa durdur
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        // çalan ses varsa durdur
        audioSource.Stop();

        // yeni moda göre başlat
        playRoutine = StartCoroutine(PlayAmbienceRoutine(isNightNow));
    }

    private IEnumerator PlayAmbienceRoutine(bool night)
    {
        isTransitioning = true;
        List<AudioClip> clips = night ? nightAmbienceClips : dayAmbienceClips;

        if (clips == null || clips.Count == 0)
        {
            Debug.LogWarning("[AmbienceManager] Geçerli mod için ses yok!");
            yield break;
        }

        isTransitioning = false;

        while (true)
        {
            // listedeki rastgele bir clip seç
            AudioClip selected = clips[Random.Range(0, clips.Count)];

            // null kontrolü
            if (selected == null)
            {
                Debug.LogWarning("[AmbienceManager] Null AudioClip bulundu.");
                yield break;
            }

            audioSource.clip = selected;
            audioSource.Play();

            // Çalma süresi boyunca bekle
            yield return new WaitForSeconds(selected.length);

            // Eğer bu sırada SetNightMode çağrılıp coroutine resetlenmişse break
            if (isTransitioning) yield break;
        }
    }
}
