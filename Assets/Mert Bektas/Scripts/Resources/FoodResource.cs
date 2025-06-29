using UnityEngine;

public class FoodResource : MonoBehaviour,IClickable, IDataPersistence
{
    public FoodData data;
    private int currentHealth;
    [SerializeField] private string uniqueID;
    [SerializeField] private bool isCollected = false;
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
            gameObject.SetActive(false);
            DropResource();
            isCollected = true;
            
        }
    }
    private void PlayHitEffects()
    {
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
            Debug.Log($"Tree NOT collected, skipping save: {uniqueID}");
        }
    }
}
