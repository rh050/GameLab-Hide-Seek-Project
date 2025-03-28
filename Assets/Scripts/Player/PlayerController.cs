using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isClone = false;
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Energy Settings")]
    public float energyCost = 2f;
    private EnergyManager energyManager;

    [Header("Animation (Future)")]
    private SpriteRenderer spriteRenderer;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);

        energyManager = EnergyManager.Instance;
        if (energyManager == null)
            Debug.LogError("EnergyManager instance not found!");

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SmartObjectManager.Instance.ActivateSmartObjects(gameObject);

        if (!IsInsideHidingSpot())
        {
            HeatmapManager.Instance.RegisterMovement(transform.position);
        }

        // שימו לב - אין כאן יותר הפעלת יכולת
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = movement * moveSpeed;
        }

        if (spriteRenderer != null && movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
            movement.Normalize();
            isMoving = true;
            Debug.Log("Movement Detected: " + movement);
        }
        else if (context.canceled)
        {
            movement = Vector2.zero;
            isMoving = false;
            Debug.Log("Movement Stopped!");
        }
    }

    private bool IsInsideHidingSpot()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("HidingSpot"))
            {
                return true;
            }
        }
        return false;
    }

    // Speed Modifiers
    public void ModifySpeed(float multiplier) => moveSpeed *= multiplier;
    public void ResetSpeed() => moveSpeed = 5f;
    public void ModifySpeedTemporary(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedChange(multiplier, duration));
    }

    private IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        ModifySpeed(multiplier);
        yield return new WaitForSeconds(duration);
        ResetSpeed();
    }
}
