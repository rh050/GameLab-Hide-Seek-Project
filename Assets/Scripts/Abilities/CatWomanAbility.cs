using UnityEngine;

[CreateAssetMenu(fileName = "Cat Woman Ability", menuName = "Ability/Cat Woman")]
public class CatWomanAbility : Ability
{
    public GameObject clonePrefab;
    public float controlDuration;
    
    void OnEnable()
    {
        if (!Application.isPlaying || DifficultyManager.Instance == null)
            return;
        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy:
                controlDuration = 15f;
                break;
            case Difficulty.Medium:
                controlDuration = 10f;
                break;
            case Difficulty.Hard:
                controlDuration = 6f;
                break;
        }
    }

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
    