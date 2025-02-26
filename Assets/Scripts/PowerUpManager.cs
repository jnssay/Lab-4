using UnityEngine;
using System.Collections;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    private bool isPoopRepellerActive = false;
    private bool isMouseGatherActive = false;
    private float gatherRange = 5.0f; // Define a range for gathering

    [SerializeField] private GameObject powerUpDisplay; // Assign the parent GameObject in the Inspector
    [SerializeField] private TMP_Text powerUpText; // Assign in the Inspector
    [SerializeField] private float displayDuration = 2f; // Set the duration in the Inspector
    [SerializeField] private ScoreUIManager scoreUIManager; // Assign in the Inspector

    private void Start()
    {
        powerUpDisplay.SetActive(false); // Ensure it's initially hidden
    }

    public void ActivatePoopRepeller(IngredientData ingredientData)
    {
        if (!isPoopRepellerActive)
        {
            isPoopRepellerActive = true;
            StartCoroutine(DeactivatePoopRepellerAfterTime(10f));
            ShowPowerUpText("Poop Repeller Activated!");

            // Update score and UI
            ScoreManager.Instance.AddScore(ingredientData.score);
            scoreUIManager.UpdateScoreUI(ingredientData.prefab.GetComponent<SpriteRenderer>().sprite, ingredientData.score);
        }
    }

    private IEnumerator DeactivatePoopRepellerAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isPoopRepellerActive = false;
    }

    public void ActivateMouseGather(IngredientData ingredientData)
    {
        if (!isMouseGatherActive)
        {
            isMouseGatherActive = true;
            StartCoroutine(DeactivateMouseGatherAfterTime(10f));
            ShowPowerUpText("Mouse Gather Activated!");

            // Update score and UI
            ScoreManager.Instance.AddScore(ingredientData.score);
            scoreUIManager.UpdateScoreUI(ingredientData.prefab.GetComponent<SpriteRenderer>().sprite, ingredientData.score);
        }
    }

    private IEnumerator DeactivateMouseGatherAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isMouseGatherActive = false;
    }

    private void Update()
    {
        if (isPoopRepellerActive)
        {
            RepelPoop();
        }

        if (isMouseGatherActive && Input.GetMouseButton(0))
        {
            GatherObjects();
        }
    }

    private void RepelPoop()
    {
        GameObject[] poops = GameObject.FindGameObjectsWithTag("Poop");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        float repelRange = 2.0f;

        foreach (GameObject poop in poops)
        {
            Rigidbody2D rb = poop.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float distance = Vector3.Distance(mousePosition, poop.transform.position);
                if (distance <= repelRange)
                {
                    Vector2 direction = (poop.transform.position - mousePosition).normalized;
                    rb.AddForce(direction * 20f, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void GatherObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Ingredient");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        foreach (GameObject obj in objects)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float distance = Vector3.Distance(mousePosition, obj.transform.position);
                if (distance <= gatherRange)
                {
                    // Move the object directly to the mouse position if it's close enough
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, mousePosition, 0.1f);

                    // Optionally, you can set the velocity to zero to stop any bouncing
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isMouseGatherActive)
        {
            Gizmos.color = Color.green;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Gizmos.DrawWireSphere(mousePosition, gatherRange);
        }
    }

    private void ShowPowerUpText(string message)
    {
        powerUpText.text = message;
        powerUpDisplay.SetActive(true);
        StartCoroutine(HidePowerUpText());
    }

    private IEnumerator HidePowerUpText()
    {
        yield return new WaitForSeconds(displayDuration);
        powerUpDisplay.SetActive(false);
    }
}