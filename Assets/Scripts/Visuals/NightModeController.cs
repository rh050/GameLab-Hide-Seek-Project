using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycleManager : MonoBehaviour
{
    [Header("Global Moonlight")]
    public Light2D globalLight;          
    [Tooltip("משך המעבר (בשניות) מ-לילה לצהריים")]
    public float cycleDuration = 100f;

    [Header("Moon → Day Gradient")]
    [Tooltip("צבע הלילה (moonlight) בצד ה־0, צבע היום בצד ה־1)")]
    public Gradient colorGradient = new Gradient()
    {
        colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(new Color(0.2f,0,0), 0f),   
            new GradientColorKey(Color.white,      1f)     
        },
        alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(0.3f, 0f),  
            new GradientAlphaKey(1f,   1f)   
        }
    };

    [Header("Intensity Curve")]
    [Tooltip("Y=עוצמה: 0 = לילה כהה, 1 = אור מלא")]
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0f, 0.3f, 1f, 1f);

    [Header("Performance")]
    [Tooltip("עדכון כל X שניות (0 = כל פריים)")]
    public float updateInterval = 0.1f;

    float lastUpdate;

    void Start()
    {
        if (globalLight == null)
            globalLight = GetComponent<Light2D>();
        UpdateCycle(1f - (GameManager.Instance.GetGameTime() / cycleDuration));
    }

    void Update()
    {
        if (updateInterval > 0f && Time.time - lastUpdate < updateInterval) return;
        lastUpdate = Time.time;

        float t = Mathf.Clamp01(1f - (GameManager.Instance.GetGameTime() / cycleDuration));
        UpdateCycle(t);
    }

    void UpdateCycle(float t)
    {
        Color c = colorGradient.Evaluate(t);
        float inten = intensityCurve.Evaluate(t);

        globalLight.color     = c;
        globalLight.intensity = inten;
    }
}