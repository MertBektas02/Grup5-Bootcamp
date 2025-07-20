using UnityEngine;
using System.Collections;
public class CowResource : MonoBehaviour, IClickable, IDataPersistence
{
    public FoodData data;
    private int currentHealth;
    [SerializeField] private string uniqueID;
    [SerializeField] private bool isCollected = false;
    void Awake()
    {
        //cow hit effect sudden color change
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    void Start()
    {
        currentHealth = data.foodHealth;
    }
    public void OnClick()
    {
        if (isCollected) return;
        PlayHitEffects();
        currentHealth--;
        if (currentHealth <= 0)
        {
            isCollected = true;
            StartCoroutine(HandleDeath());
        }
        else
        {
            // Sadece renk efekti
            Color flashColor;
            if (ColorUtility.TryParseHtmlString("#BC5F5F", out flashColor))
            {
                FlashColorEffect(flashColor, 0.2f);
            }
        }
    }
    private void PlayHitEffects()
    {
        if (floatingText != null)
        {
            ShowFloatingText();
        }

        if (data.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
        }
        if (data.hitParticlePrefab != null)
        {
            ParticleSystem ps = Instantiate(data.hitParticlePrefab, transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, 0.5f);
        }
        SoundManager.PlayRandomSound(new SoundType[]
        {
            SoundType.CowHurt1,
            SoundType.CowHurt2,
            SoundType.CowHurt3
        });
    }

    private void DropResource()
    {
        if (data.dropPrefab != null)
        {
            Instantiate(data.dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    // ---------SAVE LOAD
    public void LoadData(GameData data)
    {
        if (data.collectedObjectIDs.Contains(uniqueID))
        {
            isCollected = true;
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (isCollected && !data.collectedObjectIDs.Contains(uniqueID))
        {
            data.collectedObjectIDs.Add(uniqueID);
        }
        else
        {
            // Debug.Log($"Tree NOT collected, skipping save: {uniqueID}");
        }
    }



    [SerializeField] private GameObject floatingText;

    public void ShowFloatingText()
    {
        if (floatingText == null) return;

        Vector3 spawnPos = transform.position;
        if (Camera.main != null)
        {
            spawnPos -= Camera.main.transform.forward * 0.1f;
        }

        // instantiate world-space bağımsız
        GameObject go = Instantiate(floatingText, spawnPos, Quaternion.identity);

        // TextMesh'e currentHealth'i yaz
        TextMesh tm = go.GetComponentInChildren<TextMesh>();
        if (tm != null)
        {
            tm.text = currentHealth.ToString();
        }
    }


    //cow effects
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public void FlashColorEffect(Color flashColor, float duration)
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRoutine(flashColor, duration));
        }
    }

    private IEnumerator FlashRoutine(Color flashColor, float duration)
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
    private IEnumerator HandleDeath()
    {
        Color flashColor;
        if (ColorUtility.TryParseHtmlString("#BC5F5F", out flashColor))
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
        }

        DropResource();
        gameObject.SetActive(false);
    }
}
