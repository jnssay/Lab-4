using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class MainMenuScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetScore();
            if (currentScore > 0)
            {
                scoreText.text = "Score: " + currentScore;
            }
            else
            {
                scoreText.text = "No save data found! Start a new game!";
            }
        }
        else
        {
            scoreText.text = "No save data found! Start a new game!";
        }
    }
}