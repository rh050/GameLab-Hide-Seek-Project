using UnityEngine;

public class ObservingState : SeekerState
{
    private Vector2 targetLocation;

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now Observing.");
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (seeker.CanSeeHider())
        {
            seeker.SwitchState(new ChasingState(seeker.GetHiderTarget()));
            return;
        }
        else if (seeker.CanSeeHidingSpot())
        {
            seeker.SwitchState(new ExploringState());
            return;
        }

    }


    public void ExitState(SeekerAI seeker)
    {
        Debug.Log("Seeker is leaving Observing state.");
    }
}
