using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer; // Assign your Audio Mixer in the Inspector

    [Header("Audio Data")]
    [SerializeField] private AudioData audioData; // Assign your AudioData ScriptableObject in the Inspector

    [Header("Audio Settings")]
    [SerializeField] private float sfxMuteTime = 1.0f; // How long to mute regular SFX when power-up sounds play

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource grabSource; // New dedicated source for grab sounds
    private AudioSource explosionSource; // New dedicated source for explosion sounds
    private AudioSource powerUpSource; // New dedicated source for power-up sounds

    private float sfxOriginalVolume;
    private bool isSFXMuted = false;
    private Coroutine sfxVolumeRestoreCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            grabSource = gameObject.AddComponent<AudioSource>(); // Add the grab source
            explosionSource = gameObject.AddComponent<AudioSource>(); // Add the explosion source
            powerUpSource = gameObject.AddComponent<AudioSource>(); // Add the power-up source

            // Set the output of the AudioSources to the AudioMixer groups
            musicSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Music")[0];
            sfxSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("SFX")[0];
            grabSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Grab")[0]; // Use the Grab mixer group
            explosionSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("Explosion")[0]; // Use the Explosion mixer group
            powerUpSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("PowerUp")[0]; // Use the PowerUp mixer group

            musicSource.loop = true;

            // Store the original SFX volume
            mainMixer.GetFloat("SFXVolume", out sfxOriginalVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            Debug.Log("Playing music: " + clip.name);
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonHoverSFX()
    {
        PlaySFX(audioData.buttonHoverSFX);
    }

    public void PlayButtonClickSFX()
    {
        PlaySFX(audioData.buttonClickSFX);
    }

    public void PlayItemGrabSFX()
    {
        // Use the grab source instead of the regular SFX source
        grabSource.PlayOneShot(audioData.itemGrabSFX);
    }

    public void PlayCauldronSplashSFX()
    {
        PlaySFX(audioData.cauldronSplashSFX);
    }

    public void PlayTransitionStartSFX()
    {
        PlaySFX(audioData.transitionStartSFX);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(audioData.mainMenuMusic);
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void UnpauseMusic()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.UnPause();
        }
    }

    public void PlayPowerUpSpawnSFX()
    {
        if (audioData.powerUpSpawnSFX != null)
        {
            powerUpSource.PlayOneShot(audioData.powerUpSpawnSFX);
            MuteSFXTemporarily();
        }
    }

    public void PlayPowerUpActivateSFX()
    {
        if (audioData.powerUpActivateSFX != null)
        {
            powerUpSource.PlayOneShot(audioData.powerUpActivateSFX);
            MuteSFXTemporarily();
        }
    }

    public void PlayExplosionSFX()
    {
        if (audioData.explosionSFX != null)
        {
            explosionSource.PlayOneShot(audioData.explosionSFX);
        }
    }

    public void PlayGameOverSFX()
    {
        if (audioData.gameOverSFX != null)
        {
            // Use the power-up source for game over sound to make it louder and prioritized
            powerUpSource.PlayOneShot(audioData.gameOverSFX);
            MuteSFXTemporarily();
        }
    }

    // Method to temporarily mute regular SFX when power-up sounds play
    private void MuteSFXTemporarily()
    {
        // If we're already in the process of restoring volume, stop that coroutine
        if (sfxVolumeRestoreCoroutine != null)
        {
            StopCoroutine(sfxVolumeRestoreCoroutine);
        }

        // Mute the SFX mixer group
        mainMixer.SetFloat("SFXVolume", -80f); // -80dB is effectively muted
        isSFXMuted = true;

        // Start a coroutine to restore the volume after a delay
        sfxVolumeRestoreCoroutine = StartCoroutine(RestoreSFXVolumeAfterDelay());
    }

    private IEnumerator RestoreSFXVolumeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(sfxMuteTime);

        // Restore the original SFX volume
        mainMixer.SetFloat("SFXVolume", sfxOriginalVolume);
        isSFXMuted = false;
        sfxVolumeRestoreCoroutine = null;
    }
}