using UnityEngine;

public class TeleportActivator : MonoBehaviour
{
    public Transform[] teleportLocations; // Pre-defined teleport locations
    public float cooldown = 5f;           // Cooldown time before reuse
    private bool isOnCooldown = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hider") && !isOnCooldown)
        {
            TeleportPlayer(other.gameObject);
            StartCoroutine(StartCooldown());
        }
    }

    void TeleportPlayer(GameObject player)
    {
        if (teleportLocations.Length == 0)
            return;

        // Choose a random teleport location
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
        Debug.Log("Shortcut ready to use again!");
    }
}