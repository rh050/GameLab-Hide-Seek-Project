using UnityEngine;

public class ExploringState : SeekerState
{
    private HidingSpot targetHidingSpot;

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now Exploring.");
        targetHidingSpot = seeker.GetClosestHidingSpot();
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (seeker.CanSeeHider())
        {
            seeker.SwitchState(new ChasingState(seeker.GetHiderTarget()));
        }

        if (targetHidingSpot != null)
        {
            seeker.MoveToLocation(targetHidingSpot.transform.position);
            if (Vector3.Distance(seeker.transform.position, targetHidingSpot.transform.position) < 1f)
            {
                targetHidingSpot.BreakSpot();
                seeker.SwitchState(new ObservingState());
            }
        }
        else
        {
            seeker.SwitchState(new ObservingState());
        }
    }

    public void ExitState(SeekerAI seeker)
    {
        Debug.Log("Seeker is leaving Exploring state.");
    }
}
