using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class IngredientBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;

    // We'll track velocity based on mouse movement between frames:
    private Vector3 _lastMouseWorldPos;
    private Vector3 _currentMouseWorldPos;
    private Vector2 _calculatedVelocity;

    // To prevent the object from falling under physics while dragging:
    private bool _isDragging = false;

    // Adjust these as needed:
    [SerializeField] private float flingForceMultiplier = 10f;
    [SerializeField] private bool useGravity = false; // if top-down, set false
    [SerializeField] private float boundaryDistance = 20f; // Adjust as needed

    private int score = 0; // Initialize score

    public IngredientData ingredientData; // Make this public

    private Vector3 offset;
    private Camera mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // If you want a top-down fling without falling, set gravity to 0.
        if (!useGravity) _rb.gravityScale = 0f;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object position
        offset = transform.position - GetMouseWorldPosition();

        // Play the grab sound effect when the player starts dragging
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayItemGrabSFX();
        }

        // Switch to Kinematic so it doesn't fight us while dragging
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;

        // Store initial mouse position
        _lastMouseWorldPos = GetMouseWorldPosition();
        _isDragging = true;
    }

    private void OnMouseDrag()
    {
        // Continuously update the mouse's world position
        _currentMouseWorldPos = GetMouseWorldPosition();

        // Move the ingredient to the mouse position
        transform.position = _currentMouseWorldPos;

        // Calculate velocity based on how fast the mouse is moving
        Vector3 delta = _currentMouseWorldPos - _lastMouseWorldPos;
        _calculatedVelocity = delta / Time.deltaTime;

        // Update last position for next frame
        _lastMouseWorldPos = _currentMouseWorldPos;
    }

    void OnMouseUp()
    {
        _isDragging = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;

        // Fling velocity
        _rb.linearVelocity = _calculatedVelocity * flingForceMultiplier;

        // Apply torque impulse
        float torque = Random.Range(-10f, 10f);
        _rb.AddTorque(torque, ForceMode2D.Impulse);
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse position to world coordinates
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z; // Set the z-coordinate based on camera distance
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void Update()
    {
        if (IsOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cauldron"))
        {
            // Get the cauldron behavior component
            CauldronBehaviour cauldron = other.GetComponent<CauldronBehaviour>();
            if (cauldron != null)
            {
                // Create a splash effect and process the ingredient
                cauldron.CreateSplash(gameObject);
            }
            else
            {
                // Try the all-directions cauldron behavior
                CauldronBehaviourAllDirections cauldronAllDir = other.GetComponent<CauldronBehaviourAllDirections>();
                if (cauldronAllDir != null)
                {
                    cauldronAllDir.CreateSplash(gameObject);
                }
            }
        }
    }

    private bool IsOutOfBounds()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -boundaryDistance || screenPoint.x > 1 + boundaryDistance ||
               screenPoint.y < -boundaryDistance || screenPoint.y > 1 + boundaryDistance;
    }
}
