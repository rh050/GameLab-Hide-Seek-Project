using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fox Ability", menuName = "Ability/Fox")]
public class FoxAbility : Ability
{
    public float speedBoost = 3f;
    public float duration = 4f;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Speed boost!");
        player.GetComponent<MonoBehaviour>().StartCoroutine(BoostSpeed(player));
    }

    private IEnumerator BoostSpeed(GameObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ModifySpeed(speedBoost);
            yield return new WaitForSeconds(duration);
            pc.ResetSpeed();
        }
    }
}
