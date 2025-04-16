using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character/Create New Character")]
public class CharactersSO : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public Sprite characterSprite;
    public int speed;
    public GameObject characterPrefab;

    [Header("Ability Settings")]
    public Ability ability;

    public void ActivateAbility(GameObject player)
    {
        if (ability == null)
        {
            Debug.LogWarning("No ability assigned to " + characterName);
            return;
        }

        ability.UseAbility(player);
    }
}
