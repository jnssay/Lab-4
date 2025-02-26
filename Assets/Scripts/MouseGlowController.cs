using UnityEngine;

public class MouseGlowController : MonoBehaviour
{
    [SerializeField] private Sprite poopRepellerSprite; // Assign in the Inspector
    [SerializeField] private Sprite mouseGatherSprite;  // Assign in the Inspector

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetGlowSprite(bool isPoopRepellerActive)
    {
        if (isPoopRepellerActive)
        {
            spriteRenderer.sprite = poopRepellerSprite;
        }
        else
        {
            spriteRenderer.sprite = mouseGatherSprite;
        }
    }

    public void ActivateGlow()
    {
        spriteRenderer.enabled = true;
    }

    public void DeactivateGlow()
    {
        spriteRenderer.enabled = false;
    }
}