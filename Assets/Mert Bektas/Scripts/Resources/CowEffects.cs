using UnityEngine;
using System.Collections;

public class CowEffects : MonoBehaviour, IClickable
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {


        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            Debug.Log("SpriteRenderer bulundu. Orijinal renk: " + originalColor);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer bulunamadı!");
        }
    }



    public void FlashColorEffect(Color flashColor, float duration)
    {
        if (spriteRenderer != null)
        {
            Debug.Log("FlashColorEffect başlatıldı.");
            StartCoroutine(FlashRoutine(flashColor, duration));
        }
        else
        {
            Debug.LogWarning("FlashColorEffect: spriteRenderer yok!");
        }
    }

    private IEnumerator FlashRoutine(Color flashColor, float duration)
    {
        Debug.Log("Renk geçici olarak değiştiriliyor.");
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;
        Debug.Log("Renk eski haline döndü: " + originalColor);
    }

    public void OnClick()
    {
        Debug.Log("OnClick tetiklendi!");

        Color flashColor;
        if (ColorUtility.TryParseHtmlString("#BC5F5F", out flashColor))
        {
            Debug.Log("Renk parse edildi: " + flashColor);
            FlashColorEffect(flashColor, 0.2f);
        }
        else
        {
            Debug.LogWarning("Renk parse edilemedi!");
        }
    }

}
