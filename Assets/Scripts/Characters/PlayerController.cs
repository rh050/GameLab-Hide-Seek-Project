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
    private Vector2 LastInputX;
    private Vector2 LastInputY;
    private Vector2 InputX;
    private Vector2 InputY;
    Animator animator ;

    private PlayerCloneManager cloneManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);

        energyManager = EnergyManager.Instance;
        if (energyManager == null)
            Debug.LogError("EnergyManager instance not found!");

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get clone manager if exists
        cloneManager = GetComponent<PlayerCloneManager>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        //Prevent movement if CatWoman has active clone and is not a clone herself
        if (!isClone && cloneManager != null && cloneManager.IsCloneActive())
        {
            movement = Vector2.zero;
            return; // Block CatWoman from moving while clone is active
        }

        SmartObjectManager.Instance.ActivateSmartObjects(gameObject);

        if (!IsInsideHidingSpot())
        {
            HeatmapManager.Instance.RegisterMovement(transform.position);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = movement * moveSpeed;
        }
        //freeze Z 
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0f;
        transform.position = fixedPos;
    }

    public void Move(InputAction.CallbackContext context)
{
    Vector2 input = context.ReadValue<Vector2>().normalized;

    if (context.performed)
    {
        movement = input;
        animator.SetBool("isMoving", true);

        UpdateAnimatorFloat("InputX", input.x);
        UpdateAnimatorFloat("InputY", input.y);

        if (input != Vector2.zero)
        {
            LastInputX = new Vector2(input.x, 0);
            LastInputY = new Vector2(0, input.y);

            UpdateAnimatorFloat("LastInputX", input.x);
            UpdateAnimatorFloat("LastInputY", input.y);
        }

        Debug.Log("Movement Detected: " + movement);
    }
    else if (context.canceled)
    {
        movement = Vector2.zero;
        animator.SetBool("isMoving", false);

        UpdateAnimatorFloat("InputX", 0);
        UpdateAnimatorFloat("InputY", 0);

        Debug.Log("Movement Stopped!");
    }
}

private void UpdateAnimatorFloat(string parameter, float value)
{
    if (!Mathf.Approximately(animator.GetFloat(parameter), value))
    {
        animator.SetFloat(parameter, value);
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
