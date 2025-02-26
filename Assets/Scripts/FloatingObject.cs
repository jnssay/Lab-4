using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float floatStrength = 0.5f; // Adjust the strength of the floating effect
    [SerializeField] private float floatSpeed = 1.0f; // Adjust the speed of the floating effect

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f; // Set gravity scale to zero
        }
    }

    private void Start()
    {
        // Apply an initial random force to start the floating effect
        ApplyRandomForce();
    }

    private void ApplyRandomForce()
    {
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(
                Random.Range(-floatStrength, floatStrength),
                Random.Range(-floatStrength, floatStrength)
            );
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        // Continuously apply a small random force to simulate floating
        ApplyRandomForce();
    }
}