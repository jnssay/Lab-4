using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public void LoadLevitationScene()
    {
        // Play transition sound effect
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Wait a moment for the sound to start playing before loading the scene
        StartCoroutine(LoadSceneWithDelay("Levitation", 0.1f));
    }

    public void LoadMainMenu()
    {
        // Play transition sound effect
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Wait a moment for the sound to start playing before loading the scene
        StartCoroutine(LoadSceneWithDelay("MainMenu", 0.1f));
    }

    private System.Collections.IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        // Short delay to allow the sound to begin playing
        yield return new WaitForSeconds(delay);

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}