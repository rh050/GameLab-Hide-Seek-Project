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

        // ✅ Freeze Rigidbody of CatWoman to prevent push
        Rigidbody2D rb = realPlayer.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // full freeze while clone is active
        }

        // Enable clone control
        PlayerController cloneController = clone.GetComponent<PlayerController>();
        if (cloneController != null)
        {
            cloneController.enabled = true;
            cloneController.isClone = true; // optional
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
            // ✅ Unfreeze Rigidbody
            Rigidbody2D rb = realPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.None; // allow movement again
            }

            realPlayer.GetComponent<PlayerController>().enabled = true;

            // Return camera to follow the real player
            if (cameraFollow != null)
            {
                cameraFollow.target = realPlayer.transform;
            }
        }
    }

    public bool IsCloneActive()
    {
        return clone != null;
    }

    public Vector3 GetClonePosition()
    {
        if (clone != null)
            return clone.transform.position;
        return realPlayer.transform.position; // fallback
    }
}
