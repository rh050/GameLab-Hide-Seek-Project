using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 movement;
    private Rigidbody2D rb;
    private float originalSpeed;
    private EnergyManager energyManager;
    public float energyCost = 2f;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
        energyManager = EnergyManager.Instance;
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {

        GameMediator.Instance?.ActivateSmartObjects(gameObject);


        //ask luna game mediator to activate the hider upgrade or directly activate the seeker upgrade with PowerManager
        if (Input.GetKeyDown(KeyCode.H) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            GameMediator.Instance.ActivateHiderUpgrade(this, "invisibility");
        }

        if (Input.GetKeyDown(KeyCode.J) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            GameMediator.Instance.ActivateHiderUpgrade(this, "speed");
        }

        if (Input.GetKeyDown(KeyCode.K) && energyManager != null && energyManager.UseEnergy(energyCost))
        {
            GameMediator.Instance.ActivateSeekerUpgrade("xray");
        }
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0)
        {
            rb.MovePosition(rb.position + (Vector2)movement * moveSpeed * Time.fixedDeltaTime);
        }
    }


    public void ModifySpeed(float multiplier)
    {
        moveSpeed *= multiplier;
    }

    public void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }

    public void ModifySpeedTemporary(float multiplier, float duration)
    {
        StartCoroutine(TemporarySpeedChange(multiplier, duration));
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            movement = context.ReadValue<Vector2>();
            movement.Normalize();
            if (movement.x == 1)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (movement.x == -1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetBool("isWalking", movement.magnitude > 0);
        }
        else if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", movement.x);
            animator.SetFloat("LastInputY", movement.y);
            movement = Vector3.zero;
        }
    }

    private IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        ModifySpeed(multiplier);
        yield return new WaitForSeconds(duration);
        ResetSpeed();
    }
}