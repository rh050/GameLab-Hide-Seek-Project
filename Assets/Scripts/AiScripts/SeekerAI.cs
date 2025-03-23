using System.Collections;
using UnityEngine;

public class SeekerAI : MonoBehaviour
{
    private enum State { Observing, Exploring, Chasing }
    private State currentState = State.Observing;
    private Transform target;
    private Vector2 searchArea;
    private bool isSearchingSpot = false;
    private bool isChasing = false;
    public double DistanceHidingSpotCheck =0.5f;
    public float SeekerSpeed = 4f;


    void Start()
    {
        GameMediator.Instance.RegisterSeeker(this);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Observing:
                ObserveAndListen();
                break;
            case State.Exploring:
                ExploreHidingSpots();
                break;
            case State.Chasing:
                ChaseTarget();
                break;
        }
    }

    void ObserveAndListen()
    {
        searchArea = GameMediator.Instance.GetHottestZone();
        Debug.Log("state is observing");

        if (searchArea != Vector2.zero)
        {
            currentState = State.Exploring; 
        }
    }

    void ExploreHidingSpots()
    {
        Debug.Log("state is ExploreHidingSpots");

        if (!isSearchingSpot)
        {
            StartCoroutine(MoveToArea(searchArea));
        }
        else
        {
            CheckNearbyHidingSpots();
        }
    }

    private IEnumerator MoveToArea(Vector2 area)
    {
        isSearchingSpot = true;
        Vector3 targetPosition = new Vector3(area.x, area.y, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f);
            yield return null;
            //delete the hottest zone
            GameMediator.Instance.DeleteHottestZone(area);
        }

        isSearchingSpot = false;
    }

    private void CheckNearbyHidingSpots()
    {
        HidingSpot[] spots = FindObjectsOfType<HidingSpot>();
        foreach (HidingSpot spot in spots)
        {
            if (Vector3.Distance(transform.position, spot.transform.position) < DistanceHidingSpotCheck)
            {
                if (spot.IsOccupied)
                {//need to change 
                    Debug.Log("Found a hider in the HidingSpot!");
                    GameMediator.Instance.NotifyHiderFound(spot.GetHidingPlayer());
                    return;
                }
            }
        }

        currentState = State.Observing;
    }


    void ChaseTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * SeekerSpeed);

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                Debug.Log("Caught the hider!");
                GameMediator.Instance.NotifyHiderFound(target.gameObject.GetComponent<Hider>());
                target = null;
                currentState = State.Observing; 
            }
        }
        else
        {
            Debug.Log("Lost the hider...");
            currentState = State.Observing;
        }
    }

    public void SetChaseTarget(Transform newTarget)
    {
        target = newTarget;
        currentState = State.Chasing;
    }
}
