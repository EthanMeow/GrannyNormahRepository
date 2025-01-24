using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [Header("Collection Settings")]
    public int collectedAmount = 1; // Amount to increase when collected

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug log to check the collision
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collided object has the "Player" tag
        if (collision.CompareTag("Player"))
        {
            // Ensure GameManager.instance is not null
            if (GameManager.instance != null)
            {
                // Increase the collected amount in the GameManager or player's script
                GameManager.instance.AddToCollected(collectedAmount);

                // Debug log for collected amount
                Debug.Log($"Collected object! Total collected: {GameManager.instance.collectedAmount}");

                // Destroy the collectible object
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("GameManager instance is null. Ensure the GameManager script is present in the scene.");
            }
        }
    }
}
