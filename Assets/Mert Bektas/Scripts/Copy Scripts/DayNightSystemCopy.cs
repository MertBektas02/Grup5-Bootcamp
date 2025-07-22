using UnityEngine;

public class DayNightSystemCopy : MonoBehaviour
{
    [Header("Referanslar")]
    public GameTimeManager timeManager; // Inspector’dan bağla
    public Light sun;

    [Header("Oranlar (0-1 toplamı 1 olacak)")]
    [Range(0.1f, 0.9f)] public float dayPortion = 0.7f; // örnek: %70 gündüz, %30 gece
    [Range(0.1f, 0.9f)] public float nightPortion = 0.3f; // bu otomatik hesaplanabilir // artık otomatik hesaplanıyor lol

    [Header("Güneş Ayarları")]
    public Gradient sunColorOverTime;
    public AnimationCurve sunIntensityCurve;

    [Header("Durum Bool'ları (Sadece Okunur)")]
    [SerializeField] private bool isDay = false;
    [SerializeField] private bool isNight = false;

    public AmbienceManager ambienceManager;// isDay ve isNight bilgisini alarak hangi ambians fx'ini çalacağıma karar veriyorum.
    private bool lastIsNight = false;

    void Update()
    {
        if (timeManager == null || sun == null) return;

        float t = timeManager.GetDayProgress01();
        float shifted = (t + 0.25f) % 1f;

        isDay = shifted >= 0.25f && shifted < 0.75f;
        isNight = !isDay;

        UpdateSun(shifted);

        // Sadece gece/gündüz durumu değiştiyse haber ver
        if (isNight != lastIsNight)
        {
            if (ambienceManager != null)
            {
                ambienceManager.SetNightMode(isNight);
            }
            lastIsNight = isNight;
        }

    }

    void UpdateSun(float timeOfDay)
    {
        // Güneş açısı
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        if (sunColorOverTime != null)
            sun.color = sunColorOverTime.Evaluate(timeOfDay);

        if (sunIntensityCurve != null)
            sun.intensity = sunIntensityCurve.Evaluate(timeOfDay);

        RenderSettings.ambientLight = sun.color * 0.4f;
    }
}
