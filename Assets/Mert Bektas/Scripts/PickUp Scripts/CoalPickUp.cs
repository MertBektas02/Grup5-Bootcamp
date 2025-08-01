using UnityEngine;

public class CoalPickUp : MonoBehaviour
{
    public int amount = 1;

    public void OnPickup()
    {
        SoundManager.PlaySound(SoundType.PickUp);
        ResourceManager.Instance.AddResource(ResourceType.Coal, amount);
        Debug.Log("coal toplandı! +" + amount);
        
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
