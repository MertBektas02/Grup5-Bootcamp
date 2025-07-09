using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0.3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}