using UnityEngine;

public class Hider : MonoBehaviour
{
    private CharactersSO characterData;
    public Light playerLight;
    private bool isInHidingSpotArea = false;
    private EnergyManager energyManager;

    [Header("Ability Settings")]
    public float abilityEnergyCost = 5f;

    void Start()
    {


        if (characterData == null)
            Debug.LogWarning("CharacterData is NULL at Start!");
        if (characterData.ability == null)
            Debug.LogWarning("CharacterData.ability is NULL at Start!");
        

            GameMediator.Instance.RegisterHider(this);
        energyManager = EnergyManager.Instance;

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
        if (characterData == null || characterData.ability == null)
        {
            Debug.LogWarning("Ability not assigned!");
            return;
        }

        if (energyManager != null && energyManager.UseEnergy(abilityEnergyCost))
        {
            characterData.ActivateAbility(gameObject);
            Debug.Log(characterData.characterName + " used ability: " + characterData.ability.abilityName);
        }
        else
        {
            Debug.Log("Not enough energy to use ability!");
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
            Debug.Log("E Key Pressed");
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
