using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredientData", menuName = "Ingredient Data", order = 51)]
public class IngredientData : ScriptableObject
{
    public GameObject prefab; // The prefab for the ingredient
    public int score;         // The score value for the ingredient
    public float spawnRarity; // The spawn rarity (higher means rarer)
}