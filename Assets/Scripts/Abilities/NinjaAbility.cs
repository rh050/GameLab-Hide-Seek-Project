using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Ninja Ability", menuName = "Ability/Ninja")]
public class NinjaAbility : Ability
{
    public float invisibilityDuration = 5f;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Invisibility!");
        player.GetComponent<MonoBehaviour>().StartCoroutine(TurnInvisible(player));
    }

    private IEnumerator TurnInvisible(GameObject player)
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            yield return new WaitForSeconds(invisibilityDuration);
            sr.color = originalColor;
        }
    }
}
