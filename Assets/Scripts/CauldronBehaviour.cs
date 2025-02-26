using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class CauldronBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject splashPrefab; // Assign your splash prefab in the Inspector
    [SerializeField] private float splashOffsetY = 0.5f; // Adjust the offset as needed
    [SerializeField] private ScoreUIManager scoreUIManager; // Assign in the Inspector
    [SerializeField] private float minVerticalVelocity = -0.1f; // Minimum downward velocity to accept ingredients

    private List<string> addedIngredients = new List<string>(); // List to track added ingredient names

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is an ingredient
        if (other.CompareTag("Ingredient") || other.CompareTag("PowerUp"))
        {
            // Get the Rigidbody2D to check velocity
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            // Only process the ingredient if it's moving downward (negative y velocity)
            if (rb != null && rb.linearVelocity.y <= minVerticalVelocity)
            {
                CreateSplash(other.gameObject);
            }
        }
    }

    public void CreateSplash(GameObject ingredient)
    {
        // Instantiate splash effect with an upward offset
        Vector3 splashPosition = transform.position + new Vector3(0, splashOffsetY, 0);
        GameObject splash = Instantiate(splashPrefab, splashPosition, Quaternion.identity);

        // Play splash sound effect
        AudioManager.Instance.PlayCauldronSplashSFX();

        // Destroy the splash after 0.5 seconds
        Destroy(splash, 0.5f);

        IngredientData ingredientData = ingredient.GetComponent<IngredientBehaviour>().ingredientData;

        if (ingredient.CompareTag("PowerUp"))
        {
            PowerUpManager powerUpManager = FindObjectOfType<PowerUpManager>();
            if (powerUpManager != null)
            {
                if (ingredientData.name == "Cloud")
                {
                    // Play power-up activation sound
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayPowerUpActivateSFX();
                    }
                    powerUpManager.ActivatePoopRepeller(ingredientData);
                }
                else if (ingredientData.name == "Crystal")
                {
                    // Play power-up activation sound
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayPowerUpActivateSFX();
                    }
                    powerUpManager.ActivateMouseGather(ingredientData);
                }
                else if (ingredientData.name == "Duster")
                {
                    // Play transition sound effect
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayTransitionStartSFX();
                    }

                    // Load the "Transition" scene
                    SceneManager.LoadScene("Transition");
                }
            }
            Destroy(ingredient);
            return;
        }

        ScoreManager.Instance.AddScore(ingredientData.score);

        // Log the item, score contribution, and total score
        Debug.Log($"Item: {ingredientData.name}, Score Contribution: {ingredientData.score}, Total Score: {ScoreManager.Instance.GetScore()}");

        // Update the score UI
        scoreUIManager.UpdateScoreUI(ingredientData.prefab.GetComponent<SpriteRenderer>().sprite, ingredientData.score);

        // Add the ingredient name to the list and update the UI
        addedIngredients.Add(ingredientData.name);
        scoreUIManager.UpdateIngredientList(addedIngredients);

        Destroy(ingredient);
    }
}