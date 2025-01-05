using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 movement;
    private Rigidbody2D rb;
    private PlayerAbilities abilities;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilities>(); // חיבור לסקריפט הכוחות
    }

    void Update()
    {
        // ניהול תנועה
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // הפעלת יכולות לפי לחיצה על מקשים
        if (Input.GetKeyDown(KeyCode.E)) // התחבאות
        {
            HidePlayer();
        }

        if (Input.GetKeyDown(KeyCode.N)) // מצב לילה
        {
            ToggleNightMode();
        }

        if (Input.GetKeyDown(KeyCode.H)) // שקיפות
        {
            abilities.TurnInvisible(5f);
        }

        if (Input.GetKeyDown(KeyCode.J)) // מהירות
        {
            abilities.BoostSpeed(2f, 5f);
        }

        if (Input.GetKeyDown(KeyCode.K)) // רנטגן
        {
            abilities.ActivateXRayVision(5f);
        }

    }

    public void ModifySpeedTemporary(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedChange(multiplier, duration));
    }

    private IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        moveSpeed *= multiplier; // שינוי המהירות
        yield return new WaitForSeconds(duration); // המתנה למשך הזמן
        moveSpeed /= multiplier; // חזרה למהירות המקורית
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2)movement * moveSpeed * Time.fixedDeltaTime);
    }

    // התחבאות
    private void HidePlayer()
    {
        Debug.Log("Player is hiding!"); // ניתן לשלב כאן לוגיקה נוספת להתחבאות
    }

    // מעבר למצב לילה
    private void ToggleNightMode()
    {
        Debug.Log("Night mode toggled!");
        // לדוגמה, ניתן לשנות תאורה, רקעים או אפקטים
        RenderSettings.ambientLight = RenderSettings.ambientLight == Color.black ? Color.white : Color.black;
    }

    // שינוי מהירות זמני (נשלט ע"י abilities)
    public void ModifySpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }

    public void ResetSpeed(float originalSpeed)
    {
        moveSpeed = originalSpeed;
    }
}
