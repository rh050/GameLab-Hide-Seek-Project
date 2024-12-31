using UnityEngine;

public class Hider : MonoBehaviour
{
    void Start()
    {
        GameMediator.Instance.RegisterHider(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Seeker"))
        {
            GameMediator.Instance.NotifyHiderFound(this);
            Destroy(gameObject); // המתחבא נמצא ויוצא מהמשחק
        }
    }
}
