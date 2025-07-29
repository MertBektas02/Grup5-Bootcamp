using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public Sprite[] introImages;          // Sýrayla gösterilecek resimler
    public Image displayImage;            // UI'daki Image objesi
    public float imageDisplayTime = 13f;   // Her resmin görünme süresi
    public float fadeTime = 5f;         // Geçiþ süresi
    public string nextSceneName;          // Geçilecek sahnenin adý

    void Start()
    {
        StartCoroutine(FadeInMusic(2f)); // 2 saniyede yavaþça baþlasýn
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        for (int i = 0; i < introImages.Length; i++)
        {
            displayImage.sprite = introImages[i];

            yield return StartCoroutine(FadeImage(0f, 1f)); // Fade in
            yield return new WaitForSeconds(imageDisplayTime);
            yield return StartCoroutine(FadeImage(1f, 0f)); // Fade out
        }


        yield return StartCoroutine(FadeOutMusic(1f)); // 1 saniyede yavaþça bitsin
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = new Color(1f, 1f, 1f, displayImage.color.a);


        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            // Easing: yumuþak geçiþ
            t = t * t * (3f - 2f * t);

            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            displayImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        displayImage.color = new Color(c.r, c.g, c.b, endAlpha);
    }


    IEnumerator FadeInMusic(float duration)
    {
        float elapsed = 0f;
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0f;
        audio.Play();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audio.volume = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        audio.volume = 1f;
    }

    IEnumerator FadeOutMusic(float duration)
    {
        float elapsed = 0f;
        AudioSource audio = GetComponent<AudioSource>();
        float startVolume = audio.volume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audio.Stop();
    }

}
