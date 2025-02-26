using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableCauldron : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 lastMousePosition;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    [SerializeField] private float rotationFactor = 5f; // Adjust the rotation sensitivity
    [SerializeField] private float dragSpeed = 1.0f; // Adjust to control drag sensitivity
    [SerializeField] private bool constrainToScreen = true; // Whether to keep the object on screen
    [SerializeField] private float screenBorderPadding = 0.1f; // Padding from screen edges

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Ensure gravity is not affecting the cauldron
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Keep the same z-position
        offset = transform.position - mousePosition;

        // Start dragging
        isDragging = true;

        // Play the grab sound effect when starting to drag
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayItemGrabSFX();
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 delta = currentMousePosition - lastMousePosition;

            // Move the cauldron
            transform.position += delta;

            // Rotate the cauldron based on movement
            float rotationAmount = delta.x * rotationFactor;
            transform.Rotate(Vector3.forward, -rotationAmount);

            lastMousePosition = currentMousePosition;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Adjust for camera position
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void Update()
    {
        if (isDragging)
        {
            // Get the current mouse position in world coordinates
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // Keep the same z-position

            // Calculate the new position with the offset
            Vector3 newPosition = mousePosition + offset;

            // Apply drag speed
            Vector3 smoothPosition = Vector3.Lerp(transform.position, newPosition, dragSpeed * Time.deltaTime * 10);

            // Constrain to screen if enabled
            if (constrainToScreen)
            {
                smoothPosition = ConstrainToScreen(smoothPosition);
            }

            // Update the position
            transform.position = smoothPosition;
        }
    }

    private Vector3 ConstrainToScreen(Vector3 position)
    {
        // Convert the screen boundaries to world coordinates
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(screenBorderPadding, screenBorderPadding, position.z));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1 - screenBorderPadding, 1 - screenBorderPadding, position.z));

        // Get the object's renderer bounds
        Renderer renderer = GetComponent<Renderer>();
        float objectWidth = 0;
        float objectHeight = 0;

        if (renderer != null)
        {
            // Use the renderer's bounds
            objectWidth = renderer.bounds.extents.x;
            objectHeight = renderer.bounds.extents.y;
        }
        else
        {
            // Fallback to collider if available
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                objectWidth = collider.bounds.extents.x;
                objectHeight = collider.bounds.extents.y;
            }
        }

        // Constrain the position
        position.x = Mathf.Clamp(position.x, bottomLeft.x + objectWidth, topRight.x - objectWidth);
        position.y = Mathf.Clamp(position.y, bottomLeft.y + objectHeight, topRight.y - objectHeight);

        return position;
    }
}