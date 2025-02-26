using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] private Image ingredientImage; // Assign in the Inspector
    [SerializeField] private TMP_Text scoreText;    // Assign in the Inspector
    [SerializeField] private TMP_Text totalScoreText; // Assign in the Inspector
    [SerializeField] private TMP_Text ingredientListText; // Assign in the Inspector

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Hide the ingredient image by default
        if (ingredientImage != null)
        {
            ingredientImage.enabled = false;
        }

        // Set the default score text
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        // Set the total score text
        if (totalScoreText != null)
        {
            int totalScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;
            totalScoreText.text = $"Total Score: {totalScore}";
        }

        // Clear the ingredient list text
        if (ingredientListText != null)
        {
            ingredientListText.text = "Ingredients:\n";
        }
    }

    public void UpdateScoreUI(Sprite ingredientSprite, int score)
    {
        if (ingredientImage == null || scoreText == null || totalScoreText == null || ingredientListText == null)
        {
            Debug.LogError("UI components are not assigned in the ScoreUIManager.");
            return;
        }

        // Show the ingredient image and update the sprite
        ingredientImage.enabled = true;
        ingredientImage.sprite = ingredientSprite;

        // Update the score text
        scoreText.text = $"Score: {score}";

        // Update the total score
        totalScoreText.text = $"Total Score: {ScoreManager.Instance.GetScore()}";
    }

    public void UpdateIngredientList(List<string> ingredients)
    {
        ingredientListText.text = "Ingredients:\n" + string.Join("\n", ingredients);
    }
}