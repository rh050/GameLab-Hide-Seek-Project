using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    private enum State { Searching, Chasing, Adaptive }
    private State currentState = State.Searching;
    private HidingSpotPrediction hidingSpotPrediction;
    private Transform target;
    void Start()
    {
        hidingSpotPrediction = FindObjectOfType<HidingSpotPrediction>();
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

    void SearchForHiders()
    {
        if (currentState == State.Searching)
        {

            HidingSpot predictedSpot = hidingSpotPrediction.GetPredictedSpot();

            if (predictedSpot != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, predictedSpot.transform.position, Time.deltaTime * 2);
            }
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
        Vector2 hottestZone = HeatmapManager.Instance.GetHottestZone();
        transform.position = Vector2.MoveTowards(transform.position, hottestZone, Time.deltaTime * 2);
    }
}
