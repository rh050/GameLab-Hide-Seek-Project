using UnityEngine;

public class Hider : MonoBehaviour
{
    private HidingSpot currentSpot;
    public Light playerLight;

    void Start()
    {
        GameMediator.Instance.RegisterHider(this);

        if (playerLight != null)
        {
            playerLight.intensity = 1.0f;
            playerLight.range = 5.0f; 
            playerLight.color = Color.white; 
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!currentSpot.IsOccupied)
            {
                currentSpot.HidePlayer(gameObject);
                UpdateLight(false); 
            }
            else
            {
                currentSpot.LeaveSpot(gameObject);
                UpdateLight(true); 
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

        if (other.CompareTag("Seeker"))
        {
            GameMediator.Instance.NotifyHiderFound(this);
            Destroy(gameObject); 
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

    private void UpdateLight(bool state)
    {
        if (playerLight != null)
        {
            playerLight.enabled = state;
        }
    }
}
