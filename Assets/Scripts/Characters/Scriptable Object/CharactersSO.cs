using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character/Create New Character")]
public class CharactersSO : ScriptableObject
{
    [SerializeField] public string characterName;
    [SerializeField] public Sprite characterSprite;
    [SerializeField] public int speed;
    [SerializeField] public GameObject characterPrefab;

    [SerializeField] public Ability ability; 

    public void ActivateAbility(GameObject player)
    {
        if (ability != null)
        {
            ability.UseAbility(player);
        }
    }
}
