using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] calmAmbience;
    public AudioClip[] backgroundAmbience;
    public AudioClip[] whisperClips;
    public AudioClip[] evilLaughs;
    public AudioClip[] trapLaughs;
    public AudioClip[] gameOverClips;
    public AudioSource sfxSource;
    public AudioSource audioSource;

    private Coroutine switchCoroutine;
    private Coroutine fadeCoroutine;
    public float fadeDuration = 2f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        PlayCalmAmbience();
    }

    public void PlayCalmAmbience()
    {
        if (calmAmbience.Length > 0)
        {
            AudioClip nextClip = calmAmbience[Random.Range(0, calmAmbience.Length)];
            CrossfadeToClip(nextClip, true);

            if (switchCoroutine != null) StopCoroutine(switchCoroutine);
            switchCoroutine = StartCoroutine(SwitchToBackgroundAfterDelay(10f));
        }
    }

    IEnumerator SwitchToBackgroundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayBackgroundAmbience();
    }

    public void PlayBackgroundAmbience()
    {
        if (backgroundAmbience.Length > 0)
        {
            AudioClip nextClip = backgroundAmbience[Random.Range(0, backgroundAmbience.Length)];
            CrossfadeToClip(nextClip, true);
        }
    }

    public void PlayWhisperLoop()
    {
        if (whisperClips.Length > 0)
        {
            AudioClip nextClip = whisperClips[Random.Range(0, whisperClips.Length)];
            CrossfadeToClip(nextClip, true);
        }
    }

    public void PlayEvilLaugh()
    {
        if (evilLaughs.Length > 0)
        {
            sfxSource.PlayOneShot(evilLaughs[Random.Range(0, evilLaughs.Length)]);
        }
    }
    public void PlayTrapLaugh()
    {
        if (trapLaughs.Length > 0)
        {
            sfxSource.PlayOneShot(trapLaughs[Random.Range(0, trapLaughs.Length)]);
        }
    }

    public void PlayGameOverSFX()
    {
        if (gameOverClips.Length > 0)
        {
            sfxSource.PlayOneShot(gameOverClips[Random.Range(0, gameOverClips.Length)]);
        }
    }

    public void StopSound()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        audioSource.Stop();
    }

    // --- Core Fade Logic ---
    public void CrossfadeToClip(AudioClip newClip, bool loop)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutIn(newClip, loop));
    }

    private IEnumerator FadeOutIn(AudioClip newClip, bool loop)
    {
        float startVolume = audioSource.volume;
        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.clip = newClip;
        audioSource.loop = loop;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}
