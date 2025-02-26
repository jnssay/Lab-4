using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Cauldron"; // The scene to load after loading completes
    [SerializeField] private float minimumLoadingTime = 2f; // Minimum time to show the loading screen
    [SerializeField] private Slider progressBar; // Optional progress bar
    [SerializeField] private TMP_Text loadingText; // Optional loading text
    [SerializeField] private Button continueButton; // Button to continue to the next scene

    private bool isLoadingComplete = false;

    private void Start()
    {
        // Start the loading process
        StartCoroutine(LoadNextScene());

        // Setup the continue button if it exists
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false); // Hide the button initially
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }
    }

    private IEnumerator LoadNextScene()
    {
        // Start loading the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false; // Don't transition to the next scene automatically

        float startTime = Time.time;

        // Update progress while loading
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // AsyncOperation goes from 0 to 0.9 when loading is done

            // Update UI elements
            if (progressBar != null)
                progressBar.value = progress;

            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.Round(progress * 100)}%";

            // Check if loading is complete and minimum time has passed
            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minimumLoadingTime)
            {
                isLoadingComplete = true;

                // Show the continue button if it exists
                if (continueButton != null)
                    continueButton.gameObject.SetActive(true);
                else
                    asyncLoad.allowSceneActivation = true; // Automatically continue if there's no button
            }

            yield return null;
        }
    }

    // Called when the continue button is clicked
    public void OnContinueButtonClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Load the next scene
        if (isLoadingComplete)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // For UI button hover events - add this to your button's Event Trigger component
    public void OnButtonHover()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonHoverSFX();
        }
    }
}