using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpotPrediction : MonoBehaviour
{
    int i;
    public HidingSpot GetPredictedSpot()
    {  
        return HidingSpotManager.Instance.GetRandomAvailableSpot();
    }    
}
