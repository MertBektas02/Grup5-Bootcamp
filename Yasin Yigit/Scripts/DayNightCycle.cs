using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Zaman Ayarları")]
    [Tooltip("Tam bir günün kaç saniye süreceğini belirler.")]
    public float dayDurationInSeconds = 120f;

    [Range(0, 1)]
    [Tooltip("Günün hangi saatinde başlanacağı. 0 = gece, 0.5 = öğlen, 1 = gece")]
    public float startTimeOfDay = 0f;

    [Header("Güneş Ayarları")]
    public Light sun; // Directional Light objesi
    public Gradient sunColorOverTime;
    public AnimationCurve sunIntensityCurve;

    private float timeOfDay = 0f;

    void Start()
    {
        timeOfDay = startTimeOfDay;
    }

    void Update()
    {
        timeOfDay += Time.deltaTime / dayDurationInSeconds;
        if (timeOfDay > 1f) timeOfDay = 0f;

        UpdateSun();
    }

    void UpdateSun()
    {
        // Güneşi döndür
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Renk ve şiddet ayarları
        sun.color = sunColorOverTime.Evaluate(timeOfDay);
        sun.intensity = sunIntensityCurve.Evaluate(timeOfDay);

        // Ortam ışığını da renge göre ayarla
        RenderSettings.ambientLight = sun.color * 0.4f;
    }
}
