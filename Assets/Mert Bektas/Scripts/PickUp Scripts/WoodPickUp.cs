using UnityEngine;

public class WoodPickup : MonoBehaviour, IPickupable
{
    public int amount = 1;

    public void OnPickup()
    {
                SoundManager.PlaySound(SoundType.PickUp);

        ResourceManager.Instance.AddResource(ResourceType.Wood, amount);
        Debug.Log("Odun toplandı! +" + amount);

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
