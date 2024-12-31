using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Light playerLight;

    void Start()
    {
        if (playerLight != null)
        {
            playerLight.intensity = 1.0f;
            playerLight.range = 5.0f; // טווח הפנס
            playerLight.color = Color.white; // צבע התאורה
        }
    }
}