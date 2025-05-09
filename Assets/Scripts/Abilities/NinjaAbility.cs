﻿using UnityEngine;
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
        Hider hider = player.GetComponent<Hider>();
        if (hider == null) yield break;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            GameMediator.Instance.SetHiderInvisible(hider, true); 
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
            yield return new WaitForSeconds(invisibilityDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            GameMediator.Instance.SetHiderInvisible(hider, false); 
        }
    }

}
