using UnityEngine;
using TMPro;  // Make sure to include this for working with TextMeshPro components

public class CollectedAmountUI_TMP : MonoBehaviour
{
    public TextMeshProUGUI collectedAmountText;  // Reference to the TextMeshProUGUI component
    private GameManager gameManager;  // Reference to GameManager

    void Start()
    {
        // Get reference to GameManager
        gameManager = GameManager.instance;

        // Make sure GameManager is valid
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found!");
            return;
        }

        // Update the UI immediately
        UpdateCollectedAmountUI();
    }

    void Update()
    {
        // Continuously update the UI with the current collected amount
        if (gameManager != null)
        {
            UpdateCollectedAmountUI();
        }
    }

    // This method will update the UI text with the current collected amount
    void UpdateCollectedAmountUI()
    {
        if (collectedAmountText != null)
        {
            collectedAmountText.text = "" + gameManager.collectedAmount.ToString();
        }
    }
}
