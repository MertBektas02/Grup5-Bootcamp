using Unity.Mathematics;
using UnityEngine;

public class Tree : MonoBehaviour, IClickable
{
    public TreeData data;
    private int currentHealth;
    void Start()
    {
        currentHealth = data.treeHealth;
    }
    public void OnClick()
    {
        PlayHitEffects();
        currentHealth--;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            DropResource();
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
            Destroy(ps.gameObject, 1f);
        }
    }
    private void DropResource()
    {
        if (data.dropPrefab!=null)
        {
            Instantiate(data.dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
