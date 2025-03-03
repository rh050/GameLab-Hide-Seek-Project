using System;
using UnityEngine;

public class Hider : MonoBehaviour
{
    private HidingSpot currentSpot;
    public Light playerLight;
    private bool isInHiddingsotArea = false;


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
        if (!isInHiddingsotArea)
        {
            GameMediator.Instance.RegisterMovement(transform.position);
        }
        
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
            isInHiddingsotArea = true;
            currentSpot = other.GetComponent<HidingSpot>();
            Debug.Log("You are near a hiding spot!");
        }
       

        // if (other.CompareTag("Seeker")&& !currentSpot.IsOccupied)
        // {
        //     GameMediator.Instance.NotifyHiderSpotted(this);
        //     //destroy the hider just if its very close to the seeker
        //     if (Vector3.Distance(transform.position, other.transform.position) < 1f)
        //     {
        //         GameMediator.Instance.NotifyHiderFound(this);
        //         Destroy(gameObject);
        //     }
        // }
    }
    

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            isInHiddingsotArea = false;
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
