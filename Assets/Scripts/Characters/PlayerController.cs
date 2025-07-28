using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isClone = false;
    public Vector2 LastMoveDirection { get; private set; } = Vector2.up;


    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private float speedRegular;
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
    
    [Header("Fatigue Settings")]
    public float maxFatigue = 100f;    
    public float fatigue;             
    public float fatigueDrain = 25f;   
    public float fatigueRegen = 20f;   
    public float exhaustedSpeedMultiplier = 0.4f; 
    private bool isExhausted = false;

    public Vector2 LastDirection { get; private set; }

    private PlayerCloneManager cloneManager;

    void Start()
    {
        speedRegular = moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D is missing on " + gameObject.name);

        energyManager = EnergyManager.Instance;
        if (energyManager == null)
            Debug.LogError("EnergyManager instance not found!");

        spriteRenderer = GetComponent<SpriteRenderer>();

        cloneManager = GetComponent<PlayerCloneManager>();
        animator = GetComponent<Animator>();
        fatigue = maxFatigue;

        
    }

    void Update()
    {
        if (!isClone && cloneManager != null && cloneManager.IsCloneActive())
        {
            movement = Vector2.zero;
            return; 
        }

        SmartObjectManager.Instance.ActivateSmartObjects(gameObject);

        if (!IsInsideHidingSpot() && movement != Vector2.zero)
        {
            fatigue -= fatigueDrain * Time.deltaTime;
            if (fatigue <= 0)
            {
                fatigue = 0;
                if (!isExhausted)
                {
                    isExhausted = true;
                    moveSpeed = speedRegular * exhaustedSpeedMultiplier;
                }
            }
        }
        else
        {
            fatigue += fatigueRegen * Time.deltaTime;
            if (fatigue >= maxFatigue)
            {
                fatigue = maxFatigue;
            }

            if (isExhausted && fatigue >= maxFatigue * 0.6f) 
            {
                isExhausted = false;
                moveSpeed = speedRegular;
            }
        }
    }


    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = movement * moveSpeed;
        }
         
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
            LastMoveDirection = input;


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

    public void ModifySpeed(float multiplier) => moveSpeed *= multiplier;
    public void ResetSpeed() => moveSpeed = speedRegular;
    public void ModifySpeedTemporary(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedChange(multiplier, duration));
    }
    public void Resetfatigue()
    {
        fatigue = maxFatigue;
        isExhausted = false;
        moveSpeed = speedRegular;
    }

    private IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        
        ModifySpeed(multiplier);
        yield return new WaitForSeconds(duration);
        ResetSpeed();

        
    }
}
