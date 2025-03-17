using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cat Woman Ability", menuName = "Ability/Cat Woman")]
public class CatWomanAbility : Ability
{
    public GameObject clonePrefab;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Cloning ability!");
        GameObject clone = Instantiate(clonePrefab, player.transform.position, Quaternion.identity);
       // player.GetComponent<PlayerCloneManager>().ActivateClone(clone);
    }
}
