using UnityEngine;
using System;

public class ChasingState : SeekerState
{
    private Hider targetHider;
    private float lostSightTimer = 0f;
    private float maxLostSightDuration = 3f;
    private float originalSpeed;
    private SeekerAI mySeeker;
    private Action illusionEventHandler;



    public ChasingState(Hider target)
    {
        targetHider = target;
    }

    public void EnterState(SeekerAI seeker)
    {
        originalSpeed = seeker.moveSpeed;
        mySeeker = seeker;
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

        illusionEventHandler = OnIllusionActivated;

        if (EventManager.Instance != null)
            EventManager.Instance.OnIllusionActivated += illusionEventHandler;
    }


    private void OnIllusionActivated()
    {
        Debug.Log("Illusion event received — switching from Chasing to Exploring");
        if (mySeeker != null)
            mySeeker.SwitchState(SeekerAI.ExploringStateInstance);
    }
    public void UpdateState(SeekerAI seeker)
    {
        if (targetHider == null)
        {
            Debug.Log("Target is null – back to exploring");
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
            return;
        }

        if (!seeker.CanSeeHider(targetHider) || targetHider.CompareTag("Invisible"))
        {
            Debug.Log("Seeker lost sight of hider – switching to Exploring");
            seeker.SwitchState(SeekerAI.ExploringStateInstance);
            return;
        }

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
            return;
        }

        seeker.MoveToLocation(targetHider.transform.position);
    }

    public void ExitState(SeekerAI seeker)
    {
        seeker.moveSpeed = originalSpeed;
        if (EventManager.Instance != null && illusionEventHandler != null)
            EventManager.Instance.OnIllusionActivated -= illusionEventHandler;
        Debug.Log("Seeker is leaving Chasing state.");
    }
}
