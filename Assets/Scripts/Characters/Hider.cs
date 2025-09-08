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
    [SerializeField] private float hidingCooldownDuration = 1f;
    private float hidingCooldownTimer = 0f;

    [Header("Light Shrink Settings")]
    public float maxLightRadius = 5f;
    public float minLightRadius = 2f;
    public float shrinkRate = 0.5f;
    public float growRate = 1f;

    private CharactersSO characterData;
    private bool isInHidingSpotArea = false;
    private EnergyManager energyManager;
    public Light2D selfLight;
    private PlayerController pc;
    private bool isHiding = false;
    private HidingSpot currentHidingSpot;
    private PlayerCloneManager cloneManager;
    private Coroutine whisperCoroutine;

    void Start()
    {
        cloneManager = GetComponent<PlayerCloneManager>();
        if (!CompareTag("Clone")) { GameMediator.Instance.RegisterHider(this); }
        energyManager = EnergyManager.Instance;
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (abilityCooldownTimer > 0)
            abilityCooldownTimer -= Time.deltaTime;

        if (hidingCooldownTimer > 0)
            hidingCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            ActivateAbility();

        if (Input.GetKeyDown(KeyCode.R))
            ToggleHide();

        if (selfLight == null) return;

        if (isHiding)
        {
            if (selfLight.pointLightOuterRadius > minLightRadius)
                selfLight.pointLightOuterRadius = Mathf.Max(
                    selfLight.pointLightOuterRadius - shrinkRate * Time.deltaTime,
                    minLightRadius
                );
            else
                selfLight.enabled = false;
        }
        else
        {
            if (!selfLight.enabled)
                selfLight.enabled = true;

            if (selfLight.pointLightOuterRadius < maxLightRadius)
                selfLight.pointLightOuterRadius = Mathf.Min(
                    selfLight.pointLightOuterRadius + growRate * Time.deltaTime,
                    maxLightRadius
                );
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
            return;
        }

        if (characterData.ability == null)
        {
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
                        break;
                    }
                }
            }
        }
        else
        {
            ForceExitHiding(); 
        }
    }
    
    public void ForceExitHiding()
    {
        if (!isHiding)
            return;

        ExitHiding();
    }

    private void ExitHiding()
    {
        if (currentHidingSpot != null)
        {
            currentHidingSpot.LeaveSpot(gameObject);
            currentHidingSpot = null;
        }

        if (selfLight != null)
        {
            selfLight.enabled = true;
            selfLight.pointLightOuterRadius = minLightRadius;
        }
        SoundOnExitHide();
        pc.Resetfatigue();

        isHiding = false;
        hidingCooldownTimer = hidingCooldownDuration;
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HidingSpot"))
            isInHidingSpotArea = false;
    }

    public CharactersSO GetCharacterData() => characterData;
    public bool getisInHidingSpotArea() => isInHidingSpotArea;

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

    public bool IsHiding() => isHiding;
}
