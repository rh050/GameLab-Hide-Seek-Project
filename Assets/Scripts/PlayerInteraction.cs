using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private HidingSpot currentSpot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!currentSpot.IsOccupied)
            {
                currentSpot.HidePlayer(gameObject);
            }
            else
            {
                currentSpot.LeaveSpot(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            currentSpot = other.GetComponent<HidingSpot>();
            Debug.Log("You are near a hiding spot!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            currentSpot = null;
            Debug.Log("You left the hiding spot!");
        }
    }
}
