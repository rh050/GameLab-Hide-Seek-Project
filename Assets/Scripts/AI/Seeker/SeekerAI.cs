using System.Collections.Generic;
using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    [Header("AI Parameters (set by Difficulty)")]
    public float moveSpeed;
    public float teleportInterval;
    public float visionRadius;
    public float lostSightDuration;

    private SeekerState currentState;
    private Collider2D[] visionColliders;
    private float teleportTimer;

    // Animation-related
    private Animator animator;
    private Vector2 lastPosition;

    void Start()
    {
        // Configure parameters based on chosen difficulty
        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy:
                moveSpeed        = 2.0f;
                teleportInterval = 30.0f;
                visionRadius     = 6.0f;
                lostSightDuration= 5.0f;
                break;
            case Difficulty.Medium:
                moveSpeed        = 3.5f;
                teleportInterval = 20.0f;
                visionRadius     = 5.0f;
                lostSightDuration= 3.0f;
                break;
            case Difficulty.Hard:
                moveSpeed        = 5.0f;
                teleportInterval = 10.0f;
                visionRadius     = 4.0f;
                lostSightDuration= 1.5f;
                break;
        }

        GameMediator.Instance.RegisterSeeker(this);
        SwitchState(new ObservingState());
        animator    = GetComponent<Animator>();
        lastPosition= transform.position;
    }

    void Update()
    {
        // Teleport logic when not chasing
        teleportTimer += Time.deltaTime;
        if (!(currentState is ChasingState) && teleportTimer >= teleportInterval)
        {
            Vector2 tp = HeatmapManager.Instance.GetHottestZone();
            transform.position = tp;
            teleportTimer = 0f;
        }

        // Update state behavior
        currentState.UpdateState(this);

        // Update animations
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 movement = ((Vector2)transform.position - lastPosition).normalized;
        bool isMoving = movement.magnitude > 0.01f;

        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("InputX", movement.x);
        animator.SetFloat("InputY", movement.y);

        lastPosition = transform.position;
    }

    public void SwitchState(SeekerState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void MoveToLocation(Vector2 location)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            location,
            moveSpeed * Time.deltaTime
        );
    }

    public HidingSpot[] FindHidingSpotsNearbyOrGlobal()
    {
        var nearby = new List<HidingSpot>();
        visionColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius);

        foreach (var col in visionColliders)
        {
            if (col.CompareTag("HidingSpot"))
            {
                var spot = col.GetComponent<HidingSpot>();
                if (spot != null) nearby.Add(spot);
            }
        }

        if (nearby.Count > 0)
            return nearby.ToArray();

        // Fallback to global search
        return HidingSpotManager.Instance.GetAllActiveSpots().ToArray();
    }

    public bool CanSeeHider()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius);

        foreach (var col in visionColliders)
        {
            if (col.CompareTag("Hider") || col.CompareTag("Clone"))
            {
                var hider = col.GetComponent<Hider>();
                if (hider == null) continue;
                if (GameMediator.Instance.IsHiderInvisible(hider)) continue;
                return true;
            }
        }

        return false;
    }

    public Transform GetHiderTarget()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius);

        foreach (var col in visionColliders)
        {
            if (col.CompareTag("Hider") || col.CompareTag("Clone"))
            {
                var hider = col.GetComponent<Hider>();
                if (hider == null) continue;
                if (GameMediator.Instance.IsHiderInvisible(hider)) continue;
                return col.transform;
            }
        }

        return null;
    }

    public bool CanSeeTarget(Transform target)
    {
        // Optional: implement line-of-sight checks here
        return Vector2.Distance(transform.position, target.position) <= visionRadius;
    }
}
