using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public event Action OnIllusionActivated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerIllusionActivated()
    {
        Debug.Log("Event Triggered: IllusionActivated");
        OnIllusionActivated?.Invoke();
    }
}
