using UnityEngine;

public class ChasingState : SeekerState
{
    private Hider targetHider;
    private float lostSightTimer = 0f;
    private float maxLostSightDuration = 3f;
    private float originalSpeed;

    public ChasingState(Hider target)
    {
        targetHider = target;
    }

    public void EnterState(SeekerAI seeker)
    {
        originalSpeed = seeker.moveSpeed;
        AudioManager.Instance.PlayEvilLaugh();
        Difficulty diff = DifficultyManager.Instance != null
            ? DifficultyManager.Instance.GetDifficulty()
            : Difficulty.Medium;
        switch (diff)
        {
            case Difficulty.Easy: seeker.moveSpeed = originalSpeed * 1.2f; break;
            case Difficulty.Medium: seeker.moveSpeed = originalSpeed * 1.3f; break;
            case Difficulty.Hard: seeker.moveSpeed = originalSpeed * 1.4f; break;
        }
        lostSightTimer = 0f;
    }

    public void UpdateState(SeekerAI seeker)
    {
        if (targetHider == null)
        {
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
            return;
        }

        if (!seeker.CanSeeHider(targetHider))
        {
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
        }

        //need to transfer this to function on gamemediator (use NotifyHiderFound in game mediator)
        if (targetHider.GetComponent<Collider2D>().bounds.Contains(seeker.transform.position))
        {
            var cloneManager = targetHider.GetComponent<PlayerCloneManager>();

            if (targetHider.CompareTag("Clone"))
            {
                GameMediator.Instance.DestroyClone();
            }
            else if (cloneManager != null && cloneManager.IsCloneActive())
            {
                GameMediator.Instance.TeleportCatWomanToClone(targetHider);
            }
            else
            {
                GameMediator.Instance.NotifyHiderFound(targetHider);
            }
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
        }
        
        seeker.MoveToLocation(targetHider.transform.position);

    }

    public void ExitState(SeekerAI seeker)
    {
        seeker.moveSpeed = originalSpeed;
        Debug.Log("Seeker is leaving Chasing state.");
    }
}
