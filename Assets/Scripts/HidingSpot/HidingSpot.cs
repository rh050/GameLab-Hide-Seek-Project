using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    void Start()
    {
        GameMediator.Instance.RegisterHidingSpot(this);
    }

    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Player is hiding!");
        }
    }

    public void LeaveSpot(GameObject player)
    {
        if (IsOccupied)
        {
            IsOccupied = false;
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Player left the hiding spot!");
        }
    }

    public void BreakSpot()
    {
        IsOccupied = false;
        GameMediator.Instance.NotifyHidingSpotCollapsed(this);
        gameObject.SetActive(false);
        Debug.Log("Hiding spot has collapsed!");
    }
}
