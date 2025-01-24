using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public int playerHealth = 3;       // Player's current health
    public int maxPlayerHealth = 3;    // Maximum health the player can have

    [Header("Collectibles")]
    public int collectedAmount = 0;     // Number of collected items (collectedAmount renamed to match your request)

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Get the current player health
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    // Save the player's health
    public void SavePlayerHealth(int health)
    {
        playerHealth = Mathf.Clamp(health, 0, maxPlayerHealth);
        Debug.Log($"Player health updated to: {playerHealth}");
    }

    // Get the number of collected items
    public int GetCollectedAmount()
    {
        return collectedAmount;
    }

    // Add to the collected items
    public void AddToCollected(int amount)
    {
        collectedAmount += amount;
        Debug.Log($"Collected items updated to: {collectedAmount}");
    }

    // Save the number of collected items
    public void SaveCollectedAmount(int amount)
    {
        collectedAmount = Mathf.Max(0, amount); // Ensure the value doesn't go below 0
        Debug.Log($"Collected items updated to: {collectedAmount}");
    }

    // Reset the game state (optional, for restarts or new game logic)
    public void ResetGameState()
    {
        playerHealth = maxPlayerHealth;
        collectedAmount = 0;
        Debug.Log("Game state has been reset.");
    }
}
