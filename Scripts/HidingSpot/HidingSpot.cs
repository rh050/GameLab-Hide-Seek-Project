using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    void Start()
    {
        HidingSpotManager.Instance.RegisterHidingSpot(this);
    }

    void OnDestroy()
    {
        HidingSpotManager.Instance.UnregisterHidingSpot(this);
    }

    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            IsOccupied = true;
            playerController.enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Player is hiding!");
        }
    }

    public void LeaveSpot(GameObject player)
    {
        if (IsOccupied)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            IsOccupied = false;
            playerController.enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Player left the hiding spot!");
        }
    }
}
