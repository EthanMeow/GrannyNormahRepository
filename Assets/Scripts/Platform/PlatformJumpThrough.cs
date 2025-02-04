using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class JumpThroughPlatform : MonoBehaviour
{
    public Collider2D platformCollider; // Reference to the platform's collider
    public Transform player; // Reference to the player (assign in the Inspector)

    void Start()
    {
        // Ensure the platformCollider is assigned
        if (platformCollider == null)
        {
            platformCollider = GetComponent<Collider2D>();
        }

        if (player == null)
        {
            Debug.LogError("Player transform not assigned to JumpThroughPlatform script!");
        }
    }

    void Update()
    {
        if (player == null || platformCollider == null) return;

        // Check player's Y position relative to the platform
        if (player.position.y > transform.position.y)
        {
            // Enable the platform's collider if the player is above
            platformCollider.enabled = true;
        }
        else
        {
            // Disable the platform's collider if the player is below
            platformCollider.enabled = false;
        }
    }
}
