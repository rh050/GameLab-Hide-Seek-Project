using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
        // Get Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);
        }

        // Get EnergyManager (Singleton)
        energyManager = EnergyManager.Instance;
        if (energyManager == null)
        {
            Debug.LogError("EnergyManager instance not found!");
        }

        // Get Sprite Renderer for animation in the future
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SmartObjectManager.Instance.ActivateSmartObjects(gameObject);

        if (!IsInsideHidingSpot())
        {
            HeatmapManager.Instance.RegisterMovement(transform.position);
        }

        // Ability activation (future-proofed)
        if (Input.GetKeyDown(KeyCode.H) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            PowerManager.Instance.ActivateInvisibility(gameObject, 5f);
        }
        if (Input.GetKeyDown(KeyCode.J) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            PowerManager.Instance.ActivateSpeedBoost(gameObject, 2f, 5f);
        }
        if (Input.GetKeyDown(KeyCode.K) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            PowerManager.Instance.ActivateXRayVision(5f);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = movement * moveSpeed;
        }

        // Future animation support
        if (spriteRenderer != null)
        {
            if (movement.x != 0)
            {
                spriteRenderer.flipX = movement.x < 0;
            }
        }
    }

    // Handles movement input (New Input System)
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
            movement.Normalize();
            isMoving = true;

            Debug.Log("Movement Detected: " + movement); // ✅ Prints input values
        }
        else if (context.canceled)
        {
            movement = Vector2.zero;
            isMoving = false;

            Debug.Log("Movement Stopped!"); // ✅ Prints when movement stops
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
    public void ResetSpeed() => moveSpeed = 5f; // Reset to default
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
