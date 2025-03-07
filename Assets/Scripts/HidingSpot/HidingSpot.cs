using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; set; } = false;
    private Coroutine collapseCoroutine;
    private GameObject hiddenPlayer; 
    public float energyCost = 5f;
    private EnergyManager energyManager;

    void Start()
    {
        HidingSpotManager.Instance.RegisterHidingSpot(this);
        energyManager = EnergyManager.Instance;
    }

    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied && EnergyManager.Instance.UseEnergy(energyCost))
        {
            IsOccupied = true;
            hiddenPlayer = player; 
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Player is hiding!");
        }
        else
        {
            Debug.Log("Not enough energy to hide!");
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
        HidingSpotManager.Instance.NotifyHidingSpotCollapsed(this);
        gameObject.SetActive(false);
    }
    

    public Hider GetHidingPlayer()
    {
        if (IsOccupied)
        {
            return hiddenPlayer.GetComponent<Hider>();
        }

        return null;
    }
}
