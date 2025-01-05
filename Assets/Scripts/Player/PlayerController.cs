using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 movement;
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
            //Mediator smart objects
            GameMediator.Instance.ActivateSmartObjects(gameObject);
            GameMediator.Instance.RegisterMovement(transform.position);
            

    }
    

    void FixedUpdate()
    {
            rb.MovePosition(rb.position + (Vector2)movement * moveSpeed * Time.fixedDeltaTime);
        
    }
    
    public void ModifySpeed(float amount, float duration)
    {
        StartCoroutine(TemporarySpeedChange(amount, duration));
    }
   
    private IEnumerator TemporarySpeedChange(float amount, float duration)
    {
        moveSpeed += amount;
        yield return new WaitForSeconds(duration);
        moveSpeed -= amount;
    }
}

