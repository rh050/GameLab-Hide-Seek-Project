using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    private Coroutine collapseCoroutine;
    private GameObject hiddenPlayer; // שמירת השחקן המוסתר

    void Start()
    {
        GameMediator.Instance.RegisterHidingSpot(this);
    }

    public void HidePlayer(GameObject player)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            hiddenPlayer = player; // שמירת השחקן
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("Player is hiding!");

            // הפעלת מנגנון הקריסה לאחר 10 שניות
            collapseCoroutine = StartCoroutine(CollapseAfterDelay(10f));
        }
    }

    public void LeaveSpot(GameObject player)
    {
        if (IsOccupied)
        {
            IsOccupied = false;
            hiddenPlayer = null; // ניקוי השחקן
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Player left the hiding spot!");

            // ביטול מנגנון הקריסה אם השחקן עזב
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
            // שחרור השחקן אם המקום קורס
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
