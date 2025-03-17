using System.Collections;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateInvisibility(GameObject player, float duration)
    {
        StartCoroutine(TurnInvisible(player, duration));
    }

    private IEnumerator TurnInvisible(GameObject player, float duration)
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            yield return new WaitForSeconds(duration);
            sr.color = originalColor;
        }
    }

    public void ActivateSpeedBoost(GameObject player, float multiplier, float duration)
    {
        StartCoroutine(BoostSpeed(player, multiplier, duration));
    }

    private IEnumerator BoostSpeed(GameObject player, float multiplier, float duration)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ModifySpeed(multiplier);
            yield return new WaitForSeconds(duration);
            pc.ResetSpeed();
        }
    }

    public void ActivateXRayVision(float duration)
    {
        StartCoroutine(XRayVision(duration));
    }

    private IEnumerator XRayVision(float duration)
    {
        GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("HidingSpot");
        foreach (GameObject spot in hidingSpots)
        {
            SpriteRenderer sr = spot.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
            }
        }
        Debug.Log("X-Ray Vision activated!");
        yield return new WaitForSeconds(duration);

        foreach (GameObject spot in hidingSpots)
        {
            SpriteRenderer sr = spot.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
        Debug.Log("X-Ray Vision ended!");
    }
}