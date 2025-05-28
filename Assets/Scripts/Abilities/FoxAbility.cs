using UnityEngine;

[CreateAssetMenu(fileName = "Fox Ability", menuName = "Ability/Fox")]
public class FoxAbility : Ability
{
    [Header("Illusion Settings")]
    public GameObject illusionPrefab;
    private float illusionLifetime;

    // מיקומי אשליות בריבוע סביב השחקן
    private Vector2[] illusionOffsets = new Vector2[]
    {
        new Vector2(-1.5f,  1.5f),   // צפון-מערב
        new Vector2( 1.5f,  1.5f),   // צפון-מזרח
        new Vector2(-1.5f, -1.5f),   // דרום-מערב
        new Vector2( 1.5f, -1.5f)    // דרום-מזרח
    };

    public override void UseAbility(GameObject player)
    {
        // בחר משך חיים לפי דרגת קושי
        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy: illusionLifetime = 7f; break;
            case Difficulty.Medium: illusionLifetime = 5f; break;
            case Difficulty.Hard: illusionLifetime = 3f; break;
        }

        // שמירת כיוון התנועה האחרון של השחקן
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
            // מדלגים על מיקום אחד כדי שלא נשים על השחקן האמיתי
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
