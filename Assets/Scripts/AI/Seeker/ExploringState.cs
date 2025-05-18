using UnityEngine;

public class ExploringState : SeekerState
{
    private HidingSpot targetHidingSpot;

    public void EnterState(SeekerAI seeker)
    {
        var spots = seeker.FindHidingSpotsNearbyOrGlobal();
        if (spots.Length > 0)
        {
            targetHidingSpot = spots[Random.Range(0, spots.Length)];
        }
        else
        {
            targetHidingSpot = null;
        }
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (HeatmapManager.Instance.HasRedZones())
        {
            seeker.SwitchState(SeekerAI.ObservingStateInstance);
            return;
        }

        if (seeker.CanSeeHider(seeker.GetHiderTarget()))
        {
            seeker.SwitchState(new ChasingState(seeker.GetHiderTarget()));
            return;
        }

        if (HeatmapManager.Instance.HasRedZones())
        {  
            Vector2 nearestRedZone = GameMediator.Instance.GetNearestRedZone(seeker.transform.position);
            seeker.MoveToLocation(nearestRedZone);
        }
        else if (targetHidingSpot == null)
        {
            seeker.SwitchState(SeekerAI.ObservingStateInstance);
            return;
        }

        if (targetHidingSpot != null)
        {
            seeker.MoveToLocation(targetHidingSpot.transform.position);

            if (Vector2.Distance(seeker.transform.position, targetHidingSpot.transform.position) < 0.5f)
            {
                GameMediator.HidespotDestroyed(targetHidingSpot);
                seeker.SwitchState(SeekerAI.ExploringStateInstance);
            }
        }
        else
        {
            // אין לאן ללכת? פשוט תסתובב (אפשר להוסיף התנהגות רנדומלית)
        }
    }

    public void ExitState(SeekerAI seeker)
    {
        Debug.Log("Seeker is leaving Exploring state.");
    }
}
