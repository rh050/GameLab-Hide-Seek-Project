using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class NightModeController : MonoBehaviour
{
    public Light2D globalLight; 
    public Light2D playerLight; 
    private bool isNightMode = false;

    void Start()
    {
        // Ensure lights are in their default state
        SetNightMode(false);
    }

    void Update()
    {
        // Toggle night mode with the "N" key (example)
        if (Input.GetKeyDown(KeyCode.N))
        {
            isNightMode = !isNightMode;
            SetNightMode(isNightMode);
        }
    }

    void SetNightMode(bool active)
    {
        if (globalLight != null)
        {
            globalLight.intensity = active ? 0.0f : 1.0f; // Dim the global light in night mode
        }

        if (playerLight != null)
        {
            playerLight.enabled = active; // Enable/disable the player's light
        }

        Debug.Log($"Night mode is now {(active ? "ON" : "OFF")}");
    }
}