using UnityEngine;
using UnityEngine.UI;

public class LevitationCauldronController : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private float maxDistanceFromCamera = 15f; // Maximum allowed distance from camera
    [SerializeField] private GameObject gameOverPanel; // Assign your game over UI panel in the Inspector
    [SerializeField] private Button retryButton; // Assign the retry button from your game over panel
    [SerializeField] private Button mainMenuButton; // Assign the main menu button from your game over panel

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Scene name to load when returning to main menu
    [SerializeField] private string cauldronSceneName = "Cauldron"; // Scene name to load when retrying

    private Camera mainCamera;
    private bool isGameOver = false;

    private void Start()
    {
        mainCamera = Camera.main;

        // Ensure game over panel is initially hidden
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Set up button listeners
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    private void Update()
    {
        if (isGameOver)
            return;

        // Check distance between cauldron and camera
        float distanceFromCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // If cauldron is too far from camera, trigger game over
        if (distanceFromCamera > maxDistanceFromCamera)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Play game over sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameOverSFX();
            }
        }

        // Optionally freeze the game
        Time.timeScale = 0f;

        // Save the current score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveScore();
        }
    }

    public void RetryLevel()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Reset time scale
        Time.timeScale = 1f;

        // Load the cauldron scene instead of reloading the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(cauldronSceneName);
    }

    public void ReturnToMainMenu()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Reset time scale
        Time.timeScale = 1f;

        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
}