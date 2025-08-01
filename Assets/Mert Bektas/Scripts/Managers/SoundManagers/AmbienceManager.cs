using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;

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

        // Başlangıçta ilk sesi garanti başlat
        SetNightMode(isNightNow, true);
    }

    public void SetNightMode(bool night, bool force = false)
    {
        if (!force && isNightNow == night) return;
        isNightNow = night;

        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        audioSource.Stop();
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
            AudioClip selected = clips[Random.Range(0, clips.Count)];

            if (selected == null)
            {
                Debug.LogWarning("[AmbienceManager] Null AudioClip bulundu.");
                yield break;
            }

            audioSource.clip = selected;
            //Debug.Log("[AmbienceManager] Çalan ses: " + selected.name);
            audioSource.Play();

            yield return new WaitForSeconds(selected.length);

            if (isTransitioning) yield break;
        }
    }
}
