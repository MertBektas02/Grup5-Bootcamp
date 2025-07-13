using UnityEngine;

public class Target : MonoBehaviour
{
    public float drop = 2f;
    

    public void Commence (float amount)
    {
        drop -= amount;
        if (drop<= 0f)
        {
            Go();
        }
    }

    void Go ()
    {
        Destroy(gameObject);
    }
}
