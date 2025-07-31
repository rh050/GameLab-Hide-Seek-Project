using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SeekerAI : MonoBehaviour
{
    [Header("AI Parameters (set by Difficulty)")]
    public float moveSpeed;
    public float visionRadius;
    [Header("AI Light Growth")]
    public float seekerMaxRadius = 15f;
    private float seekerGrowRate  = 0.5f;  
    private bool HiiderInsideLightRadius = false;
    public Light2D seekerLight;
    private SeekerState currentState;
    private Collider2D[] visionColliders;
    private System.Type lastStateType = null;
    // Animation-related
    private Animator animator;
    private Vector2 lastPosition;
    //states
    public static readonly ObservingState ObservingStateInstance = new ObservingState();
    public static readonly ExploringState ExploringStateInstance = new ExploringState();
    

    void Awake()
    {
        ResetSeeker();
    }
    
    void Start()
    {
        Difficulty diff = DifficultyManager.Instance != null
            ? DifficultyManager.Instance.GetDifficulty()
            : Difficulty.Medium;
        switch (diff)
        {
            case Difficulty.Easy:
                moveSpeed         = 2.0f;
                visionRadius     = 6.0f;
                seekerGrowRate = 0.01f;
                break;
            case Difficulty.Medium:
                moveSpeed         = 2.5f;
                visionRadius     = 5.0f;
                seekerGrowRate = 0.03f;

                break;
            case Difficulty.Hard:
                moveSpeed         = 3f;
                visionRadius     = 4.0f;
                seekerGrowRate = 0.05f;
                break;
        }

        GameMediator.Instance.RegisterSeeker(this);
        SwitchState(ExploringStateInstance);
        animator    = GetComponent<Animator>();
        lastPosition= transform.position;
    }

    void Update()
    {
        if (seekerLight != null && seekerLight.pointLightOuterRadius < seekerMaxRadius)
        {
            seekerLight.pointLightOuterRadius = Mathf.Min(
                seekerLight.pointLightOuterRadius + seekerGrowRate * Time.deltaTime,
                seekerMaxRadius
            );
        }
        
        foreach (var hider in GameMediator.Instance.GetAllHiders())
        {
            if (hider == null) continue;
            if (GameMediator.Instance.IsHiderInvisible(hider)) continue;

            var hLight = hider.GetComponent<Light2D>();
            if (hLight != null && hLight.enabled)
            {
                float dist = Vector2.Distance(transform.position, hider.transform.position);
                float overlap = seekerLight.pointLightOuterRadius + hLight.pointLightOuterRadius;
                if (dist <= overlap)
                {
                    HeatmapManager.Instance.RegisterRedZone(hider);
                    break;
                }
            }
        }


        currentState.UpdateState(this);
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
        currentState?.EnterState(this);
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

        return HidingSpotManager.Instance.GetAllActiveSpots().ToArray();
    }

    public bool CanSeeHider(Hider hider)
    {
        if (hider == null) return false;
        
        if (GameMediator.Instance.IsHiderInvisible(hider))
            return false;

        if (Vector2.Distance(transform.position, hider.transform.position) > visionRadius)
        {return false;}
        else
        {
            return true;
        }
        
    }
    

    public Hider GetHiderTarget()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius);

        foreach (var col in visionColliders)
        {
            if (col.CompareTag("Hider") || col.CompareTag("Clone"))
            {
                var hider = col.GetComponent<Hider>();
                if (hider == null) continue;
                return hider;
            }
        }

        return null;
    }
    public void ResetSeeker()
    {
        currentState = new ExploringState();
        currentState.EnterState(this);
    }


    public void MarkHintedHidingSpot(HidingSpot spot)
    {
        ExploringState.targetHidingSpot = spot;
    }
}
