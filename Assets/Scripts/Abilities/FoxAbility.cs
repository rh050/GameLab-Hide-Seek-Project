using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fox Ability", menuName = "Ability/Fox")]
public class FoxAbility : Ability
{
    public float speedBoost ;
    public float duration;

    void OnEnable()
    { 
        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case Difficulty.Easy:
                speedBoost = 2.0f;
                duration   = 5.0f;
                break;
            case Difficulty.Medium:
                speedBoost = 3.0f;
                duration   = 4.0f;
                break;
            case Difficulty.Hard:
                speedBoost = 4.0f;
                duration   = 3.0f;
                break;
        }
    }
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
