using UnityEngine;
using System.Collections.Generic;

public class ExploringState : SeekerState
{
    private int requiredHidingSpots=1;
    private HidingSpot targetHidingSpot;
    

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now in ExploringState.");

        HidingSpot[] nearbySpots = seeker.GetClosestHidingSpot();
        

        if (nearbySpots.Length >= requiredHidingSpots)
        {
            int randomIndex = Random.Range(0, nearbySpots.Length);
            targetHidingSpot = nearbySpots[randomIndex].GetComponent<HidingSpot>();
        }
        else
        {
            requiredHidingSpots++;
            targetHidingSpot = null;
        }
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (seeker.CanSeeHider())
        {
            seeker.SwitchState(new ChasingState(seeker.GetHiderTarget()));
            return;
        }

        if (targetHidingSpot != null)
        {
            seeker.MoveToLocation(targetHidingSpot.transform.position);
            if (Vector3.Distance(seeker.transform.position, targetHidingSpot.transform.position) < 1f)
            {
                GameMediator.HidespotDestroyed(targetHidingSpot);
                requiredHidingSpots = 2;
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
        Debug.Log("Seeker is leaving DestroyingState.");
    }
}



