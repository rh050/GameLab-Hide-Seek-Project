using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Vector3 movement; 
    private Rigidbody2D rb; 
    private float originalSpeed; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed; 
    }

    void Update()
    {
    
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        GameMediator.Instance.RegisterMovement(transform.position);
        GameMediator.Instance.ActivateSmartObjects(gameObject);
        

        //ask luna game mediator to activate the hider upgrade or directly activate the seeker upgrade with PowerManager
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            GameMediator.Instance.ActivateHiderUpgrade(this, "invisibility"); 
        }

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            GameMediator.Instance.ActivateHiderUpgrade(this, "speed"); 
        }

        if (Input.GetKeyDown(KeyCode.K)) 
        {
            GameMediator.Instance.ActivateSeekerUpgrade("xray");
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2)movement * moveSpeed * Time.fixedDeltaTime); 
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

    private IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        ModifySpeed(multiplier); 
        yield return new WaitForSeconds(duration);
        ResetSpeed(); 
    }
}
