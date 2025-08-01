using UnityEngine;

public class StonePickUp : MonoBehaviour
{
    public int amount = 1;

    public void OnPickup()
    {
        SoundManager.PlaySound(SoundType.PickUp);
        ResourceManager.Instance.AddResource(ResourceType.Stone, amount);
        Debug.Log("stone toplandı! +" + amount);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup();
        }
    }
}
