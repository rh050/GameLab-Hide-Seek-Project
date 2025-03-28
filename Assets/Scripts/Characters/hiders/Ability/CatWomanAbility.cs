using UnityEngine;

[CreateAssetMenu(fileName = "Cat Woman Ability", menuName = "Ability/Cat Woman")]
public class CatWomanAbility : Ability
{
    public GameObject clonePrefab;
    public float controlDuration = 10f;

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Cloning ability!");

        PlayerCloneManager manager = player.GetComponent<PlayerCloneManager>();
        if (manager == null)
        {
            manager = player.gameObject.AddComponent<PlayerCloneManager>();
        }

        manager.ActivateClone(player, clonePrefab, controlDuration);
    }
}
