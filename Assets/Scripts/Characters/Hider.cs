using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Hider : MonoBehaviour
{
    
    [Header("Ability Settings")]
    [SerializeField] private float abilityEnergyCost = 5f;
    [SerializeField] private float abilityCooldownDuration = 10f;
    private float abilityCooldownTimer = 0f;

    [Header("Hiding Settings")]
    [SerializeField] private float hidingCooldownDuration = 5f;
    [SerializeField] private float minHideTime = 3f;
    [SerializeField] private float maxHideTime = 10f;
    
    [Header("Light Shrink Settings")]
    public float maxLightRadius = 5f;
    public float minLightRadius = 2f;
    public float shrinkRate = 0.5f;  
    public float growRate   = 1f;    

    private CharactersSO characterData;
    private bool isInHidingSpotArea = false;
    private EnergyManager energyManager;
    public Light2D selfLight;
    private PlayerController pc;
    private float hidingCooldownTimer = 0f;
    private bool isHiding = false;
    private HidingSpot currentHidingSpot;
    private Coroutine hideCoroutine;
    private PlayerCloneManager cloneManager;
    private Coroutine whisperCoroutine;

    

    void Start()
    {
        cloneManager = GetComponent<PlayerCloneManager>();
        if (!CompareTag("Clone")) {GameMediator.Instance.RegisterHider(this);}
        energyManager  = EnergyManager.Instance;
        pc             = GetComponent<PlayerController>();
        
    }
    
    void Update()
    {

        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }

        if (hidingCooldownTimer > 0)
        {
            hidingCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("E Key Pressed");
            ActivateAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleHide();
        }
        
        if (selfLight == null) return;

        if (isHiding)
        {
            if (selfLight.pointLightOuterRadius > minLightRadius)
            {
                selfLight.pointLightOuterRadius = Mathf.Max(
                    selfLight.pointLightOuterRadius - shrinkRate * Time.deltaTime,
                    minLightRadius
                );
            }
            else
            {
                selfLight.enabled = false;
            }
        }
        else
        {
            if (!selfLight.enabled)
                selfLight.enabled = true;

            if (selfLight.pointLightOuterRadius < maxLightRadius)
            {
                selfLight.pointLightOuterRadius = Mathf.Min(
                    selfLight.pointLightOuterRadius + growRate * Time.deltaTime,
                    maxLightRadius
                );
            }
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
        if (cloneManager != null && cloneManager.IsCloneActive())
        {
            Debug.Log("Clone is active – returning control before using ability.");
            cloneManager.ReturnControl();
            return;
        }

        if (characterData == null)
        {
            Debug.LogWarning("CharacterData is null. Cannot use ability.");
            return;
        }

        if (characterData.ability == null)
        {
            Debug.LogWarning("Ability is not assigned to characterData.");
            return;
        }

        if (abilityCooldownTimer > 0)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (energyManager != null && energyManager.UseEnergy(abilityEnergyCost))
        {
            characterData.ActivateAbility(gameObject);
            Debug.Log($"{characterData.characterName} used ability: {characterData.ability.abilityName}");
            abilityCooldownTimer = abilityCooldownDuration;
        }
        else
        {
            Debug.Log("Not enough energy to use ability.");
        }
    }
    

    void ToggleHide()
    {
        if (!isHiding)
        {
            if (hidingCooldownTimer > 0)
            {
                Debug.Log("Hiding is on cooldown! Time left: " + hidingCooldownTimer.ToString("F1") + "s");
                return;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var col in colliders)
            {
                if (col.CompareTag("HidingSpot"))
                {
                    HidingSpot spot = col.GetComponent<HidingSpot>();
                    if (spot != null)
                    {
                        spot.HidePlayer(gameObject);
                        isHiding = true;
                        currentHidingSpot = spot;
                        SoundOnEnterHide();

                        float hideDuration = Random.Range(minHideTime, maxHideTime);
                        hideCoroutine = StartCoroutine(EndHideAfterSeconds(hideDuration));
                        break;
                    }
                }
            }
        }
        else
        {
            ExitHiding();
        }
    }

    void ExitHiding()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (currentHidingSpot != null)
        {
            currentHidingSpot.LeaveSpot(gameObject);
        }
        
        if (selfLight != null)
        {
            selfLight.enabled = true;
            selfLight.pointLightOuterRadius = minLightRadius; 
        }
        SoundOnExitHide();

        isHiding = false;
        currentHidingSpot = null;
        hidingCooldownTimer = hidingCooldownDuration;

        Debug.Log("Exited hiding. Cooldown started.");
    }

    private IEnumerator EndHideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ExitHiding();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            isInHidingSpotArea = true;
            Debug.Log("You are near a hiding spot!");
        }

        if (other.CompareTag("Seeker") && CompareTag("Clone"))
        {
            PlayerCloneManager cloneManager = GetComponent<PlayerCloneManager>();
            if (cloneManager != null)
            {
                cloneManager.ReturnControl();
                Debug.Log("Clone touched the Seeker — returning control to CatWoman.");
            }
        }
    }
    public CharactersSO GetCharacterData()
    {
        return characterData;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            isInHidingSpotArea = false;
        }
    }
     public bool getisInHidingSpotArea()
     {
         return isInHidingSpotArea;
     }   
     
    void SoundOnEnterHide()
    {
        if (whisperCoroutine != null) StopCoroutine(whisperCoroutine);
        AudioManager.Instance.PlayCalmAmbience();
        whisperCoroutine = StartCoroutine(StartWhisperAfterDelay(5f)); 
    }

    IEnumerator StartWhisperAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.StopSound();
        AudioManager.Instance.PlayBackgroundAmbience();
        AudioManager.Instance.PlayWhisperLoop();
    }

    void SoundOnExitHide()
    {
        if (whisperCoroutine != null) StopCoroutine(whisperCoroutine);
        AudioManager.Instance.StopSound();
        AudioManager.Instance.PlayCalmAmbience(); 
    }


}
