using UnityEngine;
public class HeadlightController : MonoBehaviour
{
    public Light[] headlights; // iki farı inspector’dan bağla
    private bool isOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // L tuşu ile far aç/kapa
        {
            isOn = !isOn;
            foreach (var light in headlights)
            {
                light.enabled = isOn;
            }
        }
    }
}