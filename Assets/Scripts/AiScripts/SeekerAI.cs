using System.Collections;
using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    private enum State { Searching, Chasing, Adaptive }
    private State currentState = State.Searching;

    private Transform target;

    void Start()
    {
        GameMediator.Instance.RegisterSeeker(this);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Searching:
                SearchForHiders();
                break;
            case State.Chasing:
                ChaseTarget();
                break;
            case State.Adaptive:
                AdaptSearchPattern();
                break;
        }
    }


    public void ActivateXRayVision(float duration)
    {
        StartCoroutine(EnableXRay(duration));
    }

    private IEnumerator EnableXRay(float duration)
    {
        foreach (Hider hider in GameMediator.Instance.GetAllHiders())
        {
            SpriteRenderer sr = hider.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.red; 
            }
        }

        yield return new WaitForSeconds(duration);

        foreach (Hider hider in GameMediator.Instance.GetAllHiders())
        {
            SpriteRenderer sr = hider.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.white; 
            }
        }
    }
    
    //state machine
    void SearchForHiders()
    {
        HidingSpot predictedSpot = GameMediator.Instance.GetRandomAvailableSpot();
        if (predictedSpot != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, predictedSpot.transform.position, Time.deltaTime * 2);
        }
    }

    void ChaseTarget()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 3);
        }
    }

    void AdaptSearchPattern()
    {
        GameMediator.Instance.DisplayHeatmap();
        Vector2 hottestZone = HeatmapManager.Instance.GetHottestZone();
        transform.position = Vector2.MoveTowards(transform.position, hottestZone, Time.deltaTime * 2);
    }
}
