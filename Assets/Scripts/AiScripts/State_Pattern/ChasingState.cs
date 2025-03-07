using UnityEngine;

public class ChasingState : SeekerState
{
    private Transform targetHider;

    public ChasingState(Transform target)
    {
        targetHider = target;
    }

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now Chasing.");
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (targetHider != null)
        {
            seeker.MoveToLocation(targetHider.position);

            if (Vector3.Distance(seeker.transform.position, targetHider.position) < 1f)
            {
                GameMediator.Instance.NotifyHiderFound(targetHider.GetComponent<Hider>());
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
        Debug.Log("Seeker is leaving Chasing state.");
    }
}
