using UnityEngine;

public class Hider : MonoBehaviour
{
    private CharactersSO characterData;
    public Light playerLight;
    private bool isInHidingSpotArea = false;

    void Start()
    {
        GameMediator.Instance.RegisterHider(this);

        if (playerLight != null)
        {
            playerLight.intensity = 1.0f;
            playerLight.range = 5.0f;
            playerLight.color = Color.white;
        }
    }

    public void AssignCharacter(CharactersSO selectedCharacter)
    {
        characterData = selectedCharacter;
        GetComponent<SpriteRenderer>().sprite = characterData.characterSprite;
        Debug.Log("Hider assigned: " + characterData.characterName);
    }

    public void ActivateAbility()
    {
        if (characterData != null && characterData.ability != null)
        {
            Debug.Log(characterData.characterName + " used ability: " + characterData.ability.abilityName);
            characterData.ActivateAbility(gameObject);
        }
    }

    void Update()
    {
        
        if (!isInHidingSpotArea)
        {
            HeatmapManager.Instance.RegisterMovement(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateAbility();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            isInHidingSpotArea = true;
            Debug.Log("You are near a hiding spot!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            isInHidingSpotArea = false;
        }
    }
}
