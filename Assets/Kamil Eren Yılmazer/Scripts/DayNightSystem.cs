using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    [Header("Zaman Ayarları")]
    [Tooltip("Gündüz süresi (saniye cinsinden)")]
    public float dayDuration = 900f; // 15 dakika

    [Tooltip("Gece süresi (saniye cinsinden)")]
    public float nightDuration = 300f; // 5 dakika

    [Range(0, 1)]
    [Tooltip("Günün hangi saatinde başlanacağı. 0 = gece, 0.5 = öğlen, 1 = gece")]
    public float startTimeOfDay = 0f;

    [Header("Güneş Ayarları")]
    public Light sun;
    public Gradient sunColorOverTime;
    public AnimationCurve sunIntensityCurve;

    private float timeOfDay = 0f;

    void Start()
    {
        timeOfDay = startTimeOfDay;
    }

    void Update()
    {
        float deltaTimeFraction;

        // Gündüz: 0.25 - 0.75 (yarım dairelik gündüz aralığı)
        if (timeOfDay >= 0.25f && timeOfDay < 0.75f)
        {
            // Gündüz süresi 50% zaman aralığını kapsıyor
            deltaTimeFraction = (Time.deltaTime / dayDuration) * 0.5f;
        }
        else
        {
            // Gece süresi 50% zaman aralığını kapsıyor
            deltaTimeFraction = (Time.deltaTime / nightDuration) * 0.5f;
        }

        timeOfDay += deltaTimeFraction;
        if (timeOfDay > 1f) timeOfDay -= 1f;

        UpdateSun();
    }

    void UpdateSun()
    {
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        sun.color = sunColorOverTime.Evaluate(timeOfDay);
        sun.intensity = sunIntensityCurve.Evaluate(timeOfDay);

        RenderSettings.ambientLight = sun.color * 0.4f;
    }
}
