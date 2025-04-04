using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    private SeekerState currentState;
    private Transform currentTarget;
    private Collider2D[] visionColliders;
    

    void Start()
    {
        GameMediator.Instance.RegisterSeeker(this);
        SwitchState(new ObservingState());
    }

    void Update()
    {
        currentState.UpdateState(this);
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
        transform.position = Vector2.MoveTowards(transform.position, location, Time.deltaTime * 2);
    }

    public bool CanSeeHidingSpot()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if (col.CompareTag("HidingSpot"))
            {
                return true;
            }
        }
        return false;
    }

    public HidingSpot GetClosestHidingSpot()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if (col.CompareTag("HidingSpot"))
            {
                return col.GetComponent<HidingSpot>();
            }
        }
        return null;
    }

    public bool CanSeeHider()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if (col.CompareTag("Hider"))
            {
                return true;
            }
        }
        return false;
    }
    
    public void SetChaseTarget(Transform target)
    {
        SwitchState(new ChasingState(target));
    }


    public Transform GetHiderTarget()
    {
        visionColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in visionColliders)
        {
            if (col.CompareTag("Hider"))
            {
                return col.transform;
            }
        }
        return null;
    }
    public bool CanSeeTarget(Transform target) { 
        //achikam need to implement this
        /*
        if (target == null) return false;
        // If target has invisibility enabled, we can’t see them
        InvisibilityController invis = target.GetComponent<InvisibilityController>();
        if (invis != null && invis.IsInvisible)
            return false;

        Vector2 direction = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 5f);

        if (hit.collider != null && hit.collider.transform == target)
            return true;
            */

        return false;
        
    }

    

}
