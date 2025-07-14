using UnityEngine;

public class WoodPickup : MonoBehaviour, IPickupable
{
    public int amount = 1;

    public void OnPickup()
    {
        ResourceManager.Instance.AddResource(ResourceType.Wood, amount);
        Debug.Log("Odun toplandı! +" + amount);
        // TODO: Envantere ekle
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
