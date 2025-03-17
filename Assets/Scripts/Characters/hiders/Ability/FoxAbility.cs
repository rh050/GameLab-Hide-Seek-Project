using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fox Ability", menuName = "Ability/Fox")]
public class FoxAbility : Ability
{
    public float speedBoost = 3f;
    public float duration = 4f;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Speed boost!");
        PowerManager.Instance.ActivateSpeedBoost(player, speedBoost, duration);
    }
}
