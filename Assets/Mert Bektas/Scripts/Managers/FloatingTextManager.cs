using System.Numerics;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public float destroyTime = 1.12f;
    public UnityEngine.Vector3 Offset = new UnityEngine.Vector3(0, 2, 0);
    public UnityEngine.Vector3 RandomizeIntensity=new UnityEngine.Vector3(1,0,0);

    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += Offset;
        transform.localPosition += new UnityEngine.Vector3(
            Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z)
            );

    }

    
    void LateUpdate()
    {
        // Kameraya bak
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

}
