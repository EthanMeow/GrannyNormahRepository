using UnityEngine;
using TMPro;

public class InteractWithNPC : MonoBehaviour
{
    public float interactionDistance = 3f; // The maximum distance for interaction
    public GameObject interactionUI; // Reference to the UI object for speech
    public TMP_Text speechText; // Reference to the TextMeshPro component where speech will be displayed
    public string interactionText = "Hello! I'm GrannyNorma, nice to meet you!"; // Public variable to change text in Inspector

    public GameObject player; // Manually assign the player GameObject in the Inspector
    private bool isInRange = false; // Boolean to check if the player is in range of the object

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player object is not assigned in the Inspector!");
            return;
        }

        if (interactionUI != null)
        {
            interactionUI.SetActive(false); // Hide UI initially
        }
    }

    void Update()
    {
        if (player == null || interactionUI == null) return;

        // Check if the player is close enough to the object
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (!isInRange)
            {
                isInRange = true;
                Debug.Log("Player is in range of the object.");
            }

            // If the player presses 'E' and is within range
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                Debug.Log("Player interacted with the object.");
            }
        }
        else if (isInRange)
        {
            isInRange = false;
            Debug.Log("Player is out of interaction range.");
        }
    }

    void Interact()
    {
        // If the interaction UI is already active, do nothing
        if (interactionUI.activeSelf) return;

        // Activate UI and display the speech text
        interactionUI.SetActive(true);
        speechText.text = interactionText; // Use the customizable text from the Inspector

        // Attempt to get the CharacterController2D from the player
        var playerController = player.GetComponent<CharacterController2D>();
        if (playerController != null)
        {
            playerController.isSpeakingToNPC = true; // Set the flag to true to lock movement
            Debug.Log("Player movement locked.");
        }
        else
        {
            Debug.LogError("CharacterController2D component not found on player!");
        }
    }

    // Public method to close the interaction UI
    public void CloseInteraction()
    {
        if (interactionUI == null) return;

        interactionUI.SetActive(false); // Hide the interaction UI
        speechText.text = ""; // Clear the speech text

        // Reset the 'isSpeakingToNPC' flag
        var playerController = player.GetComponent<CharacterController2D>();
        if (playerController != null)
        {
            playerController.isSpeakingToNPC = false; // Unlock player movement
            Debug.Log("Player movement unlocked.");
        }
        else
        {
            Debug.LogError("CharacterController2D component not found on player!");
        }
    }
}
