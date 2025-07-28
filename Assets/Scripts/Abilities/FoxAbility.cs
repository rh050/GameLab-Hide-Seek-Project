using UnityEngine;

[CreateAssetMenu(fileName = "Fox Ability", menuName = "Ability/Fox")]
public class FoxAbility : Ability
{
    public GameObject illusionPrefab;
    private float illusionLifetime;

    private Vector2[] illusionOffsets = new Vector2[]
    {
        new Vector2(-1.5f,  1.5f),   
        new Vector2( 1.5f,  1.5f),   
        new Vector2(-1.5f, -1.5f),   
        new Vector2( 1.5f, -1.5f)    
    };

    public override void UseAbility(GameObject player)
    {
        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy: illusionLifetime = 7f; break;
            case Difficulty.Medium: illusionLifetime = 5f; break;
            case Difficulty.Hard: illusionLifetime = 3f; break;
        }

        var controller = player.GetComponent<PlayerController>();
        Vector2 dir = (controller != null && controller.LastMoveDirection != Vector2.zero)
                       ? controller.LastMoveDirection.normalized
                       : Vector2.up;

        CreateIllusions(player.transform.position, dir);
        EventManager.Instance.TriggerIllusionActivated();
    }

    private void CreateIllusions(Vector2 playerPos, Vector2 direction)
    {
        int idx = 0;
        for (int i = 0; i < illusionOffsets.Length; i++)
        {
            if (i == 2) continue;

            Vector2 spawn = playerPos + illusionOffsets[i];
            var illusion = Instantiate(illusionPrefab, spawn, Quaternion.identity);
            illusion.tag = "Clone";

            var beh = illusion.GetComponent<IllusionFoxBehavior>();
            if (beh != null)
            {
                beh.lifetime = illusionLifetime;
                beh.SetDirection(direction);
            }

            Destroy(illusion, illusionLifetime);
            idx++;
        }
    }
}
