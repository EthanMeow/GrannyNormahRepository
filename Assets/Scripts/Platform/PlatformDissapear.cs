using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PlatformDisappear : MonoBehaviour
{
    public float disappearDelay = 2f; // Time in seconds before the platform becomes non-collidable
    public float reappearDelay = 3f; // Time in seconds before the platform becomes collidable again
    public float minOpacity = 0.2f; // Minimum opacity before the platform disappears

    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer; // To control the platform's opacity
    private JumpThroughPlatform jumpThroughPlatform; // Reference to the JumpThroughPlatform script
    private bool isPlayerOnPlatform = false;

    void Start()
    {
        // Get the platform's Collider2D and SpriteRenderer
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<SpriteRenderer>();
        jumpThroughPlatform = GetComponent<JumpThroughPlatform>();

        if (platformCollider == null)
        {
            Debug.LogError("Platform is missing a Collider2D component!");
        }
        if (platformRenderer == null)
        {
            Debug.LogError("Platform is missing a SpriteRenderer component!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player lands on the platform
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            StopAllCoroutines(); // Stop any existing coroutines
            StartCoroutine(HandlePlatformDisappearance());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the platform
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }

    private IEnumerator HandlePlatformDisappearance()
    {
        float elapsedTime = 0f;

        // Gradually reduce opacity during the disappear delay
        while (elapsedTime < disappearDelay)
        {
            elapsedTime += Time.deltaTime;
            float opacity = Mathf.Lerp(1f, minOpacity, elapsedTime / disappearDelay);

            if (platformRenderer != null)
            {
                Color color = platformRenderer.color;
                color.a = opacity;
                platformRenderer.color = color;
            }

            yield return null;
        }

        // Disable the JumpThroughPlatform script
        if (jumpThroughPlatform != null)
        {
            jumpThroughPlatform.enabled = false;
        }

        // Make the platform non-collidable and invisible
        platformCollider.enabled = false;
        if (platformRenderer != null)
        {
            Color color = platformRenderer.color;
            color.a = 0f;
            platformRenderer.color = color;
        }

        // Wait for the reappear delay
        yield return new WaitForSeconds(reappearDelay);

        // Make the platform visible again and restore opacity
        platformCollider.enabled = true;
        if (platformRenderer != null)
        {
            Color color = platformRenderer.color;
            color.a = 1f;
            platformRenderer.color = color;
        }

        // Re-enable the JumpThroughPlatform script
        if (jumpThroughPlatform != null)
        {
            jumpThroughPlatform.enabled = true;
        }
    }
}
