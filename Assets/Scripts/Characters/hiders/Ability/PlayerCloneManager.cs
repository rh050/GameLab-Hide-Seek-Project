using Unity.VisualScripting;
using UnityEngine;

public class PlayerCloneManager : MonoBehaviour
{
    private GameObject realPlayer;
    private GameObject clone;
    private CameraFollow cameraFollow;

    public void ActivateClone(GameObject player, GameObject clonePrefab, float controlDuration)
    {
        realPlayer = player;

        // Instantiate clone
        clone = Instantiate(clonePrefab, player.transform.position, Quaternion.identity);

        // Disable real player control
        realPlayer.GetComponent<PlayerController>().enabled = false;

        // Enable clone control
        PlayerController cloneController = clone.GetComponent<PlayerController>();
        if (cloneController != null)
        {
            cloneController.enabled = true;
            cloneController.isClone = true; // optional if you want to mark clones
        }

        // Get camera follow script
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = clone.transform; // make camera follow the clone
        }

        // Start returning control after duration
        StartCoroutine(ReturnControlAfterDelay(controlDuration));
    }

    private System.Collections.IEnumerator ReturnControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnControl();
    }

    public void ReturnControl()
    {
        if (clone != null)
        {
            Destroy(clone);
        }

        if (realPlayer != null)
        {
            realPlayer.GetComponent<PlayerController>().enabled = true;

            // Return camera to follow the real player
            if (cameraFollow != null)
            {
                cameraFollow.target = realPlayer.transform;
            }
        }
    }
}
