using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic; // Assign the scene-specific music in the Inspector

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(sceneMusic); // Play the scene's background music
        }
    }
}