using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    public static SmartObjectManager Instance;
    private List<SmartObject> smartObjects = new List<SmartObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterSmartObject(SmartObject smartObject)
    {
        smartObjects.Add(smartObject);
    }

    public void ActivateSmartObjects(GameObject player)
    {
        foreach (SmartObject smartObject in smartObjects)
        {
            if (Vector3.Distance(player.transform.position, smartObject.transform.position) < 0.5f)
            {
                smartObject.Activate(player);
            }
        }
    }
}
