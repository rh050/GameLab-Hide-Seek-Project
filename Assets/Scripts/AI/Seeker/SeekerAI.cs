using System.Collections.Generic;
using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    private SeekerState currentState;
    private Transform currentTarget;
    private Collider2D[] visionColliders;
    public float teleportInterval = 20f;
    private float teleportTimer = 0f;
    public float moveSpeed = 3.5f;

    // Animation-related
    private Animator animator;
    private Vector2 lastInputX;
    private Vector2 lastInputY;
    private Vector2 inputX;
    private Vector2 inputY;
    private Vector2 lastPosition;
    
    void Start()
    {
        GameMediator.Instance.RegisterSeeker(this);
        SwitchState(new ObservingState());
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        teleportTimer += Time.deltaTime;
        if (!(currentState is ChasingState) && teleportTimer >= teleportInterval)
        {
            Vector2 teleportPosition = HeatmapManager.Instance.GetHottestZone();
            transform.position = teleportPosition;
            teleportTimer = 0f;
        }

        currentState.UpdateState(this);

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 movement = (Vector2)transform.position - lastPosition;
        movement = movement.normalized;

        inputX = new Vector2(movement.x, 0);
        inputY = new Vector2(0, movement.y);

        bool isMoving = movement.magnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        animator.SetFloat("InputX", movement.x);
        animator.SetFloat("InputY", movement.y);

        if (isMoving)
        {
            lastInputX = new Vector2(movement.x, 0);
            lastInputY = new Vector2(0, movement.y);

            animator.SetFloat("LastInputX", movement.x);
            animator.SetFloat("LastInputY", movement.y);
        }

        lastPosition = transform.position;
    }

    public void SwitchState(SeekerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public void MoveToLocation(Vector2 location)
    {
        transform.position = Vector2.MoveTowards(transform.position, location, Time.deltaTime * moveSpeed);

    }
    

    public HidingSpot[] FindHidingSpotsNearbyOrGlobal()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        List<HidingSpot> nearbySpots = new List<HidingSpot>();

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("HidingSpot"))
            {
                HidingSpot spot = col.GetComponent<HidingSpot>();
                if (spot != null)
                {
                    nearbySpots.Add(spot);
                }
            }
        }

        if (nearbySpots.Count > 0)
        {
            return nearbySpots.ToArray(); 
        }

        Debug.Log("No nearby hiding spots. Searching globally...");
        List<HidingSpot> allSpots = HidingSpotManager.Instance.GetAllActiveSpots();
        return allSpots.ToArray();
    }


    public bool CanSeeHider()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if ((col.CompareTag("Hider") || col.CompareTag("Clone")))
            {
                Hider hider = col.GetComponent<Hider>();
                if (hider == null) continue;

                if (GameMediator.Instance.IsHiderInvisible(hider))
                    continue; // ⬅️ מתעלם ממנו

                return true;
            }
        }
        return false;
    }




    public Transform GetHiderTarget()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if ((col.CompareTag("Hider") || col.CompareTag("Clone")))
            {
                Hider hider = col.GetComponent<Hider>();
                if (hider == null) continue;

                if (GameMediator.Instance.IsHiderInvisible(hider))
                    continue;

                return col.transform;
            }
        }
        return null;
    }


    public bool CanSeeTarget(Transform target)
    {
        // Implement your line-of-sight logic if needed
        return false;
    }



}
