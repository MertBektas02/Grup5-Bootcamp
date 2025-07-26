using UnityEngine;

public class DynamicHeadlights : MonoBehaviour
{
    public Light[] headlights;       // Far ışıkları
    public Rigidbody vehicleBody;    // Aracın rigidbody’si
    public float minIntensity = 1f;  // Dururkenki yoğunluk
    public float maxIntensity = 3f;  // Maksimum yoğunluk
    public float maxSpeed = 50f;     // Bu hıza ulaştığında maxIntensity olur

    void Update()
    {
        float speed = vehicleBody.linearVelocity.magnitude; // aracın anlık hızı
        // 0 ile maxSpeed arasında normalize et
        float t = Mathf.InverseLerp(0, maxSpeed, speed);
        // Intensity'yi hesapla
        float currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        foreach (var light in headlights)
        {
            light.intensity = currentIntensity;
        }
    }
}