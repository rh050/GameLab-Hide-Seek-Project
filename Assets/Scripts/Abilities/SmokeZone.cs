using System.Collections.Generic;
using UnityEngine;

public class SmokeZone : MonoBehaviour
{
    private readonly List<GameObject> hidersInside = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hider"))
        {
            other.tag = "Invisible";
            hidersInside.Add(other.gameObject);
            Debug.Log("Hider entered smoke - now invisible");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Invisible"))
        {
            other.tag = "Hider";
            hidersInside.Remove(other.gameObject);
            Debug.Log("Hider exited smoke - now visible again");
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject hider in hidersInside)
        {
            if (hider != null && hider.CompareTag("Invisible"))
                hider.tag = "Hider";
        }
        hidersInside.Clear();
    }
}
