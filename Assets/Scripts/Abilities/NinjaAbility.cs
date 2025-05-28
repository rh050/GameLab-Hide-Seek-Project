using UnityEngine;

[CreateAssetMenu(fileName = "Ninja Ability", menuName = "Ability/Ninja")]
public class NinjaAbility : Ability
{
    [Header("Smoke Settings")]
    public GameObject smokeZonePrefab;
    public float smokeDuration = 5f;

    void OnEnable()
    {
        if (!Application.isPlaying || DifficultyManager.Instance == null)
            return;

        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy:
                smokeDuration = 7.0f;
                break;
            case Difficulty.Medium:
                smokeDuration = 5.0f;
                break;
            case Difficulty.Hard:
                smokeDuration = 3.0f;
                break;
        }
    }

    public override void UseAbility(GameObject player)
    {
        Debug.Log(abilityName + " activated: Smoke Bomb!");

        if (smokeZonePrefab == null)
        {
            Debug.LogWarning("SmokeZone prefab not assigned to NinjaAbility.");
            return;
        }

        Vector3 spawnPosition = player.transform.position;
        GameObject smoke = Instantiate(smokeZonePrefab, spawnPosition, Quaternion.identity);
        Destroy(smoke, smokeDuration);
    }
}
