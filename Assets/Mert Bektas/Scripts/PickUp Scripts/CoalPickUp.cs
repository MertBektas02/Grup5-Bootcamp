using UnityEngine;

public class CoalPickUp : MonoBehaviour
{
    public int amount = 1;

    public void OnPickup()
    {
        ResourceManager.Instance.AddResource(ResourceType.Coal, amount);
        Debug.Log("coal toplandı! +" + amount);
        SoundManager.PlaySound(SoundType.PickUp);
        
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
