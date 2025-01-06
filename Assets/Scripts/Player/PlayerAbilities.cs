using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private float originalSpeed;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSpeed = playerController.moveSpeed; // שמירת מהירות מקורית
    }

    // הפיכת שחקן לשקוף
    public void TurnInvisible(float duration)
    {
        StartCoroutine(BecomeInvisible(duration));
    }

    private IEnumerator BecomeInvisible(float duration)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); // שקיפות חלקית
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor; // חזרה לצבע המקורי
    }

    // שינוי מהירות זמני
    public void BoostSpeed(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedBoost(multiplier, duration));
    }

    private IEnumerator TemporarySpeedBoost(float multiplier, float duration)
    {
        playerController.ModifySpeed(multiplier); // הגברת מהירות
        yield return new WaitForSeconds(duration);
        playerController.ResetSpeed(originalSpeed); // חזרה למהירות המקורית
    }

    // הפעלת רנטגן
    public void ActivateXRayVision(float duration)
    {
        StartCoroutine(XRayVision(duration));
    }

    private IEnumerator XRayVision(float duration)
    {
        // הפיכת אובייקטים מתאימים לשקופים
        GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("HidingSpot");
        foreach (GameObject spot in hidingSpots)
        {
            SpriteRenderer spotRenderer = spot.GetComponent<SpriteRenderer>();
            if (spotRenderer != null)
            {
                spotRenderer.color = new Color(spotRenderer.color.r, spotRenderer.color.g, spotRenderer.color.b, 0.3f); // שקיפות
            }
        }

        Debug.Log("X-Ray Vision activated!");
        yield return new WaitForSeconds(duration);

        // ביטול השקיפות
        foreach (GameObject spot in hidingSpots)
        {
            SpriteRenderer spotRenderer = spot.GetComponent<SpriteRenderer>();
            if (spotRenderer != null)
            {
                spotRenderer.color = new Color(spotRenderer.color.r, spotRenderer.color.g, spotRenderer.color.b, 1f); // חזרה למצב המקורי
            }
        }

        Debug.Log("X-Ray Vision ended!");
    }
}
