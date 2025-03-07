using UnityEngine;

public class ObservingState : SeekerState
{
    private Vector2 targetLocation;

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now Observing.");
        targetLocation = HeatmapManager.Instance.GetHottestZone();
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (seeker.CanSeeHidingSpot())
        {
            seeker.SwitchState(new ExploringState());
        }

        if (seeker.CanSeeHider())
        {
            seeker.SwitchState(new ChasingState(seeker.GetHiderTarget()));
        }

        if (targetLocation != Vector2.zero)
        {
            seeker.MoveToLocation(targetLocation);
            if (Vector2.Distance(seeker.transform.position, targetLocation) < 1f)
            {
                HeatmapManager.Instance.DeleteHottestZone(targetLocation);
                targetLocation = HeatmapManager.Instance.GetHottestZone();
            }
        }
    }

    public void ExitState(SeekerAI seeker)
    {
        Debug.Log("Seeker is leaving Observing state.");
    }
}
