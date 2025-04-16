using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator animator;
    private bool isTriggered = false;
    public float resetDelay = 3f; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Hider"))
        {
            isTriggered = true;
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        animator.SetTrigger("Activate");
        StartCoroutine(ResetTrapAfterDelay());
    }

    private IEnumerator ResetTrapAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        animator.ResetTrigger("Activate"); 
        animator.SetTrigger("Reset"); 
        isTriggered = false;
    }
}
