using System.Collections;
using UnityEngine;

public class ObservingState : SeekerState
{
    private Vector2 targetLocation = Vector2.positiveInfinity;
    private bool isWaiting = false;
    

    public void EnterState(SeekerAI seeker)
    {
        if (HeatmapManager.Instance.HasRedZones())
        {
            targetLocation = HeatmapManager.Instance.GetNearestRedZone(seeker.transform.position);
        }
        else
        {
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
        }
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (isWaiting) return; 

        if (seeker.CanSeeHider( seeker.GetHiderTarget()))
        {
            seeker.SwitchState( new ChasingState(seeker.GetHiderTarget()));
            return;
        }


        if (targetLocation == Vector2.positiveInfinity)
        {
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
            return;
        }

        if (Vector2.Distance(seeker.transform.position, targetLocation) < 0.1f)
        {
            HeatmapManager.Instance.RemoveRedZone(targetLocation);
            isWaiting = true;
            seeker.StartCoroutine(WaitAndSwitch(seeker));
        }
        seeker.MoveToLocation(targetLocation);
    }

    private IEnumerator WaitAndSwitch(SeekerAI seeker)
    {
        yield return new WaitForSeconds(2f);
        isWaiting = false;
        seeker.SwitchState(SeekerAI.ExploringStateInstance);
    }

    public void ExitState(SeekerAI seeker)
    {
        isWaiting = false;
        Debug.Log("Seeker is leaving Observing state.");
    }
}
