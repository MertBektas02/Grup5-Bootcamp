using UnityEngine;

public class WaterResource : MonoBehaviour, IClickable, IDataPersistence
{
    public WaterData data;
    private int currentHealth;
    [SerializeField] private string uniqueID;
    [SerializeField] private bool isCollected = false;
    void Start()
    {
        currentHealth = data.waterHealth;
    }

    public void OnClick()
    {
        if (isCollected) return;
        PlayHitEffects();
        currentHealth--;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            DropResource();
            isCollected = true;
            Debug.Log(isCollected);
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
    }
    private void DropResource()
    {
        if (data.dropPrefab != null)
        {
            Instantiate(data.dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

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

            //Debug.Log($"Tree NOT collected, skipping save: {uniqueID}");
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
}
