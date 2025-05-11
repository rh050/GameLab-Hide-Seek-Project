// DayNightCycleManager.cs
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycleManager : MonoBehaviour
{
    [Header("Light References")]
    public Light2D globalLight;
    public Light2D[] additionalLights;

    [Header("Cycle Settings")]
    public float cycleDuration = 100f;

    [Header("Appearance Curves")]
    public Gradient colorGradient;
    public AnimationCurve intensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Performance")]
    public float updateInterval = 0.1f;

    private float lastUpdateTime = 0f;

    void Start()
    {
        if (globalLight == null)
            globalLight = GetComponent<Light2D>();

        UpdateCycle();
    }

    void Update()
    {
        if (updateInterval > 0f && Time.time - lastUpdateTime < updateInterval)
            return;

        lastUpdateTime = Time.time;
        UpdateCycle();
    }

    private void UpdateCycle()
    {
        float timeLeft = GameManager.Instance.GetGameTime();
        float t = Mathf.Clamp01(1f - (timeLeft / cycleDuration));

        float intensity = intensityCurve.Evaluate(t);
        Color color     = colorGradient.Evaluate(t);

        ApplyTo(globalLight, intensity, color);

        if (additionalLights != null)
        {
            foreach (var lt in additionalLights)
                ApplyTo(lt, intensity, color);
        }
    }

    private void ApplyTo(Light2D lt, float intensity, Color color)
    {
        if (lt == null) return;
        lt.intensity = intensity;
        lt.color     = color;
    }
}
