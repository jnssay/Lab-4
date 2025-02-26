using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Assign in the Inspector
    [SerializeField] private Button resumeButton; // Assign in the Inspector
    [SerializeField] private Button mainMenuButton; // Assign in the Inspector
    [SerializeField] private Button restartButton; // Assign in the Inspector

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Ensure the pause menu is initially hidden

        // Add button click listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        // Add hover listeners using EventTriggers
        AddHoverListener(resumeButton);
        AddHoverListener(mainMenuButton);
        AddHoverListener(restartButton);
    }

    private void AddHoverListener(Button button)
    {
        if (button != null)
        {
            // Add EventTrigger component if it doesn't exist
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            // Create entry for pointer enter event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnButtonHover(); });
            trigger.triggers.Add(entry);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Make sure to unpause the music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnpauseMusic();
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Pause music but keep SFX active
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PauseMusic();
        }
    }

    public void ReturnToMainMenu()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Make sure to reset time scale before changing scenes
        Time.timeScale = 1f;

        // Make sure music is unpaused before changing scenes
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnpauseMusic();
        }

        ScoreManager.Instance.SaveScore(); // Save the score
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void RestartLevel()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }

        // Make sure to reset time scale before restarting
        Time.timeScale = 1f;

        // Make sure music is unpaused before restarting
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnpauseMusic();
        }

        ScoreManager.Instance.ResetScore(); // Reset the score to 0
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // For UI button hover events
    public void OnButtonHover()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonHoverSFX();
        }
    }
}