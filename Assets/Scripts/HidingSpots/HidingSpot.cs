using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private GameObject hiddenPlayer;
    private Hider hidingPlayerScript;

    [Header("Timing & Difficulty")]
    public float maxHideTime = 8f;
    private float hideTimer = 0f;

    [Header("Visual Alert (Red Light / Effect)")]
    public GameObject alertEffectPrefab;   
    public float alertEffectDuration = 3f;
    private GameObject currentAlertEffect;

    [Header("Energy")]
    public float energyCost = 5f;
    private EnergyManager energyManager;

    void Start()
    {
        HidingSpotManager.Instance.RegisterHidingSpot(this);
        energyManager = EnergyManager.Instance;

        Difficulty diff = DifficultyManager.Instance != null
            ? DifficultyManager.Instance.GetDifficulty()
            : Difficulty.Medium;
        switch (diff)
        {
            case Difficulty.Easy:    maxHideTime = 12f; break;
            case Difficulty.Medium:  maxHideTime = 8f;  break;
            case Difficulty.Hard:    maxHideTime = 5f;  break;
        }
    }

    void Update()
    {
        if (IsOccupied && hiddenPlayer != null && hidingPlayerScript != null)
        {
            hideTimer += Time.deltaTime;

            if (hideTimer > maxHideTime)
            {
                hidingPlayerScript.ForceExitHiding();
                ShowAlertEffect();
            }
        }
        else
        {
            hideTimer = 0f;
        }
    }


    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied && energyManager.UseEnergy(energyCost))
        {
            IsOccupied = true;
            hiddenPlayer = player;
            hidingPlayerScript = player.GetComponent<Hider>();

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
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
            hidingPlayerScript = null;

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;

            Debug.Log("Player left the hiding spot!");
        }
    }


    public void BreakSpot()
    {
        if (IsOccupied && hiddenPlayer != null)
        {
            if (hidingPlayerScript != null)
                hidingPlayerScript.ForceExitHiding();
        }
        IsOccupied = false;
        hiddenPlayer = null;
        hidingPlayerScript = null;
        HidingSpotManager.Instance.UnregisterHidingSpot(this);
        Destroy(gameObject);
    }

 
    private void ShowAlertEffect()
    {
        if (alertEffectPrefab != null)
        {
            currentAlertEffect = Instantiate(
                alertEffectPrefab,
                transform.position + Vector3.up * 1f,
                Quaternion.identity,
                transform
            );
            Destroy(currentAlertEffect, alertEffectDuration);
        }
    }


    public Hider GetHidingPlayer()
    {
        return hidingPlayerScript;
    }
}
