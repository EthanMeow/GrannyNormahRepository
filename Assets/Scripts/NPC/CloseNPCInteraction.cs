using UnityEngine;

public class CloseInteractionButton : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player; // Reference to the player GameObject
    public GameObject objectToDeactivate; // The object to deactivate

    /// <summary>
    /// Call this method to set isSpeakingToNPC to false and deactivate an object.
    /// </summary>
    public void CloseInteraction()
    {
        // Ensure the player object is assigned
        if (player == null)
        {
            Debug.LogError("Player object is not assigned!");
            return;
        }

        // Get the CharacterController2D component from the player
        var playerController = player.GetComponent<CharacterController2D>();
        if (playerController != null)
        {
            // Set isSpeakingToNPC to false
            playerController.isSpeakingToNPC = false;
            Debug.Log("Player is no longer speaking to an NPC.");
        }
        else
        {
            Debug.LogError("CharacterController2D component not found on the player!");
        }

        // Deactivate the specified object
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            Debug.Log($"Object '{objectToDeactivate.name}' has been deactivated.");
        }
        else
        {
            Debug.LogWarning("No object specified to deactivate.");
        }
    }
}
