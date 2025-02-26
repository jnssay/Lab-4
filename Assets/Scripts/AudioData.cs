using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    [Header("Background Music")]
    public AudioClip mainMenuMusic;
    public AudioClip cauldronMusic;
    public AudioClip levitationMusic;

    [Header("Sound Effects")]
    public AudioClip buttonHoverSFX;
    public AudioClip buttonClickSFX;
    public AudioClip itemGrabSFX;
    public AudioClip cauldronSplashSFX;
    public AudioClip transitionStartSFX;

    [Header("Power Up Sound Effects")]
    public AudioClip powerUpSpawnSFX;
    public AudioClip powerUpActivateSFX;

    [Header("Explosion Sound Effects")]
    public AudioClip explosionSFX;

    [Header("Game State Sound Effects")]
    public AudioClip gameOverSFX;
}