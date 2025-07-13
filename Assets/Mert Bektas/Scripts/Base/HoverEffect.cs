using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI; 


//oha scale büyütme efektinin bir ismi varmış. pointer denen zımbırtı da çok pratikmiş.
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScaleMultiplier = 1.5f; 
    private Vector3 originalScale; 

    void Start()
    {
        originalScale = transform.localScale; 
    }

    // Mouse üzerine geldiğinde
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScaleMultiplier;
        SoundManager.PlaySound(SoundType.ButtonHoverEffect);
    }

    // Mouse üzerinden çekildiğinde
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // Orijinal boyuta dön
    }
}