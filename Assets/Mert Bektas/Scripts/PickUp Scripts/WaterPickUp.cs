using UnityEngine;

public class WaterPickUp : MonoBehaviour
{
    public int amount = 1;

    public void OnPickup()
    {
        ResourceManager.Instance.AddResource(ResourceType.Water, amount);
        Debug.Log("water toplandı! +" + amount);
        
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
