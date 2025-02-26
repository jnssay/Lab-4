using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class CauldronBehaviourAllDirections : MonoBehaviour
{
    [SerializeField] private GameObject splashPrefab; // Assign your splash prefab in the Inspector
    [SerializeField] private float splashOffsetY = 0.5f; // Adjust the offset as needed
    [SerializeField] private ScoreUIManager scoreUIManager; // Assign in the Inspector

    private List<string> addedIngredients = new List<string>(); // List to track added ingredient names

    private void Awake()
    {
        // Ensure the cauldron has an EdgeCollider2D component
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        if (edgeCollider == null)
        {
            edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        // Define points for the edge collider to encompass all directions
        Vector2[] points = new Vector2[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
            new Vector2(-1, -1) // Close the loop
        };
        edgeCollider.points = points;
    }

    public void CreateSplash(GameObject ingredient)
    {
        // Instantiate splash effect with an upward offset
        Vector3 splashPosition = transform.position + new Vector3(0, splashOffsetY, 0);
        GameObject splash = Instantiate(splashPrefab, splashPosition, Quaternion.identity);

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
                    powerUpManager.ActivatePoopRepeller(ingredientData);
                }
                else if (ingredientData.name == "Crystal")
                {
                    powerUpManager.ActivateMouseGather(ingredientData);
                }
                else if (ingredientData.name == "Duster")
                {
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