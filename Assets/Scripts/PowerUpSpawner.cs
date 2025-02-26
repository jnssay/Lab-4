using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopwerUpSpawner : MonoBehaviour
{
    [Header("Ingredient Data")]
    public List<IngredientData> ingredients; // List of ingredient data

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Where the ingredient spawns
    public Vector3 spawnScale = Vector3.one; // Scale factor for spawned ingredients
    public float spawnInterval = 1f; // Interval between spawns, set in Inspector

    [Header("Launch Parameters")]
    public float minForce = 5f;  // Minimum impulse force
    public float maxForce = 10f; // Maximum impulse force
    public float minAngle = 30f; // Minimum launch angle (degrees)
    public float maxAngle = 60f; // Maximum launch angle (degrees)

    // Start spawning when the scene begins.
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Spawns 1-3 ingredients every second.
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Randomize how many ingredients to launch (1 to 3).
            int spawnCount = Random.Range(1, 4); // max is exclusive, so 4 means 1 to 3.
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnAndLaunchIngredient();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Instantiates and launches a single ingredient in an arc.
    private void SpawnAndLaunchIngredient()
    {
        IngredientData selectedIngredient = SelectRandomIngredient();
        if (selectedIngredient != null)
        {
            // Instantiate the ingredient at the spawn point.
            GameObject ingredient = Instantiate(selectedIngredient.prefab, spawnPoint.position, Quaternion.identity);
            // Play power-up spawn sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPowerUpSpawnSFX();
            }
            // Apply the scale factor
            ingredient.transform.localScale = spawnScale;

            // Get the Rigidbody2D component.
            Rigidbody2D rb = ingredient.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Randomize the force and angle.
                float force = Random.Range(minForce, maxForce);
                float angle = Random.Range(minAngle, maxAngle);

                // Convert angle from degrees to radians.
                float angleRad = angle * Mathf.Deg2Rad;

                // Calculate the launch direction.
                Vector2 launchDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

                // Apply the force as an impulse to launch the ingredient.
                rb.AddForce(launchDirection * force, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("Ingredient prefab does not have a Rigidbody2D component attached.");
            }
        }
    }

    private IngredientData SelectRandomIngredient()
    {
        float totalRarity = 0f;
        foreach (var ingredient in ingredients)
        {
            totalRarity += 1f / ingredient.spawnRarity;
        }

        float randomValue = Random.Range(0f, totalRarity);
        float cumulativeRarity = 0f;

        foreach (var ingredient in ingredients)
        {
            cumulativeRarity += 1f / ingredient.spawnRarity;
            if (randomValue <= cumulativeRarity)
            {
                return ingredient;
            }
        }

        return null;
    }
}