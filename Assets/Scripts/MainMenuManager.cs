using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Import the UI namespace

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string loadingSceneName = "Loading"; // Set this to your game scene name
    [SerializeField] private string cauldronSceneName = "Cauldron"; // Set this to your cauldron scene name
    [SerializeField] private TMP_Text scoreText; // Assign in the Inspector
    [SerializeField] private Button continueButton; // Assign in the Inspector

    private void Start()
    {
        // Play main menu music using the AudioManager
        if (AudioManager.Instance != null)
        {
            // This will use the mainMenuMusic clip from the AudioData Scriptable Object
            AudioManager.Instance.PlayMainMenuMusic();
        }

        ScoreManager.Instance.LoadScore(); // Load the score
        UpdateScoreDisplay();
        UpdateContinueButtonState();
    }

    public void StartNewGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Reset the score for a new game
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(loadingSceneName);
    }

    public void ContinueGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Directly load the cauldron scene to continue the game
        SceneManager.LoadScene(loadingSceneName);
    }

    public void QuitGame()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        Application.Quit();
    }

    // For UI button hover events
    public void OnButtonHover()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonHoverSFX();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            int currentScore = ScoreManager.Instance.GetScore();
            scoreText.text = "Score: " + currentScore;
            Debug.Log("Score displayed: " + currentScore);
        }
    }

    private void UpdateContinueButtonState()
    {
        if (continueButton != null)
        {
            bool canContinue = ScoreManager.Instance != null && ScoreManager.Instance.GetScore() > 0;
            continueButton.interactable = canContinue;
        }
    }
}
