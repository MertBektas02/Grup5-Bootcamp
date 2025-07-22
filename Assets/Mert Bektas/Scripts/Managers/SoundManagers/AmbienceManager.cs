using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource daySource;
    [SerializeField] private AudioSource nightSource;

    [Header("Ambience Clips")]
    [SerializeField] private List<AudioClip> dayAmbienceClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> nightAmbienceClips = new List<AudioClip>();

    [Header("Settings")]
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 15f;
    [SerializeField] private float crossfadeDuration = 2f;

    private bool isNight = false;
    private Coroutine ambienceRoutine;

    private void Start()
    {
        // Gündüz ile başla
        StartAmbienceRoutine(dayAmbienceClips, daySource);
    }
    void Update()
    {
        Debug.Log($"isnight:"+isNight);
    }

    public void SetNightMode(bool night)
    {
        if (isNight == night) return; // Zaten aynı moddaysa değişme
        isNight = night;

        // önce mevcut routine’i durdur
        if (ambienceRoutine != null)
            StopCoroutine(ambienceRoutine);

        if (isNight)
        {
            // Fade Day -> Night
            StartCoroutine(Crossfade(daySource, nightSource, crossfadeDuration));
            ambienceRoutine = StartCoroutine(AmbienceRoutine(nightAmbienceClips, nightSource));
        }
        else
        {
            // Fade Night -> Day
            StartCoroutine(Crossfade(nightSource, daySource, crossfadeDuration));
            ambienceRoutine = StartCoroutine(AmbienceRoutine(dayAmbienceClips, daySource));
        }
    }

    private void StartAmbienceRoutine(List<AudioClip> clips, AudioSource source)
    {
        ambienceRoutine = StartCoroutine(AmbienceRoutine(clips, source));
    }

    private IEnumerator AmbienceRoutine(List<AudioClip> clips, AudioSource source)
    {
        while (true)
        {
            if (clips.Count > 0)
            {
                var clip = clips[Random.Range(0, clips.Count)];
                Debug.Log($"Playing clip: {clip.name} at time: {Time.time}");
                source.PlayOneShot(clip);

                // Klip süresi kadar bekle ki ses üst üste binmesin
                yield return new WaitForSeconds(clip.length);

                // Ardından rastgele bir gecikme ekle
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
            }
            else
            {
                // Klip yoksa biraz bekle
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator Crossfade(AudioSource from, AudioSource to, float duration)
    {
        float time = 0f;
        float fromStartVol = from.volume;
        float toStartVol = to.volume;

        to.volume = 0f;
        to.Play(); // Loop açık olmalı

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            from.volume = Mathf.Lerp(fromStartVol, 0f, t);
            to.volume = Mathf.Lerp(0f, toStartVol, t);
            yield return null;
        }

        from.Stop();
    }
}