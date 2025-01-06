using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private Coroutine collapseCoroutine;
    private GameObject hiddenPlayer; 

    void Start()
    {
        GameMediator.Instance.RegisterHidingSpot(this);
    }

    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            hiddenPlayer = player; 
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Player is hiding!");

            collapseCoroutine = StartCoroutine(CollapseAfterDelay(10f));
        }
        else
        {
            Debug.Log("Hiding spot is already occupied!");
        }
    }

    public void LeaveSpot(GameObject player)
    {
        if (IsOccupied && hiddenPlayer == player)
        {
            IsOccupied = false;
            hiddenPlayer = null; 
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Player left the hiding spot!");

            if (collapseCoroutine != null)
            {
                StopCoroutine(collapseCoroutine);
            }
        }
    }

    public void BreakSpot()
    {
        if (IsOccupied && hiddenPlayer != null)
        {
            LeaveSpot(hiddenPlayer); 
        }

        IsOccupied = false;
        GameMediator.Instance.NotifyHidingSpotCollapsed(this);
        gameObject.SetActive(false);
        Debug.Log("Hiding spot has collapsed!");
    }

    private IEnumerator CollapseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BreakSpot(); 
    }
}
