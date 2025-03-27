using UnityEngine;

public class ChasingState : SeekerState
{
    private Transform targetHider;
    private float lostSightTimer = 0f; 
    private float maxLostSightDuration = 3f;

    public ChasingState(Transform target)
    {
        targetHider = target;
    }

    public void EnterState(SeekerAI seeker)
    {
        Debug.Log("Seeker is now Chasing.");
        lostSightTimer = 0f;
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (targetHider == null)
        {
            seeker.SwitchState(new ExploringState());
            return;
        }

        if (!seeker.CanSeeTarget(targetHider))
        {
            lostSightTimer += Time.deltaTime;

            if (lostSightTimer >= maxLostSightDuration)
            {
                Debug.Log("Seeker lost the target.");
                seeker.SwitchState(new ExploringState());
                return;
            }
        }
        else
        {
            lostSightTimer = 0f;
        }

        seeker.MoveToLocation(targetHider.position);

        if (Vector3.Distance(seeker.transform.position, targetHider.position) < 1f)
        {
            GameMediator.Instance.NotifyHiderFound(targetHider.GetComponent<Hider>());
            seeker.SwitchState(new ObservingState());
        }
    }

    public void ExitState(SeekerAI seeker)
    {
        Debug.Log("Seeker is leaving Chasing state.");
    }
}
