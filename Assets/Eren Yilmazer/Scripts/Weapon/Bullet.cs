using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float lifetime = 3f;
    
    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

}