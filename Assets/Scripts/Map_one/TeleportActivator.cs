using UnityEngine;

public class TeleportActivator : MonoBehaviour
{
    public Transform[] teleportLocations; // Pre-defined teleport locations
    public float cooldown = 5f;           // Cooldown time before reuse
    public float energyCost = 10f;        // Energy cost for teleportation
    private bool isOnCooldown = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hider") && !isOnCooldown)
        {
            if (EnergyManager.Instance.UseEnergy(energyCost))
            {
                TeleportPlayer(other.gameObject);
                StartCoroutine(StartCooldown());
            }
            else
            {
                Debug.Log("Not enough energy to teleport!");
            }
        }
    }

    void TeleportPlayer(GameObject player)
    {
        if (teleportLocations.Length == 0) return;

        Transform destination = teleportLocations[Random.Range(0, teleportLocations.Length)];
        player.transform.position = destination.position;
        Debug.Log("Player teleported to: " + destination.name);
    }
    
    System.Collections.IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        Debug.Log("Teleport on cooldown...");
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
        Debug.Log("Teleport ready to use again!");
    }
}