using UnityEngine;

public class RaycastClicker : MonoBehaviour
{
    public float maxDistance = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("Tıkladığın obje: " + hit.collider.name);
            }
        }
    }
}