using UnityEngine;

public class FoodPickUp : MonoBehaviour
{
    public int amount = 1;

    public void OnPickup()
    {
        ResourceManager.Instance.AddResource(ResourceType.Food, amount);
        Debug.Log("food toplandı! +" + amount);
        
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
