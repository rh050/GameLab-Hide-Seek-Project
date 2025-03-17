using UnityEngine;

[CreateAssetMenu(fileName = "Ninja Ability", menuName = "Ability/Ninja")]
public class NinjaAbility : Ability
{
    public float invisibilityDuration = 5f;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Invisibility!");
        PowerManager.Instance.ActivateInvisibility(player, invisibilityDuration);
    }
}
