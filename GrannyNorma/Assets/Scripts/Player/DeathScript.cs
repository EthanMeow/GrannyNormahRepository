using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad; // The name of the scene to load
    public float sceneLoadDelay = 2f; // Delay before loading the scene

    [Header("Player Reference")]
    public CharacterController2D player; // Reference to the player object
    public CameraFollow cameraFollow;  // Reference to the CameraFollow script for zoom functionality

    [Header("UI Fade Effect")]
    public GameObject uiToFade;  // Reference to the GameObject containing UI elements to fade
    public float fadeDuration = 2f; // Duration for the fade effect
    public float fadeStartDelay = 2f; // Delay before starting the fade effect

    private bool isPlayerDead = false;

    void Update()
    {
        // Get current health from the GameManager
        int currentHealth = GameManager.instance != null ? GameManager.instance.GetPlayerHealth() : -1;

        if (currentHealth <= 0 && !isPlayerDead)
        {
            isPlayerDead = true;
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        Debug.Log("Player has died. Starting death sequence.");

        // Lock player movement by setting isSpeakingToNPC to true
        if (player != null)
        {
            player.isSpeakingToNPC = true; // Lock player movement when dead
        }

        // Zoom in on the player when they die
        if (cameraFollow != null)
        {
            Debug.Log("Calling ZoomIn function.");
            cameraFollow.ZoomIn(); // Call the ZoomIn method from the CameraFollow script
        }

        // Start the fade effect after a delay
        if (uiToFade != null)
        {
            StartCoroutine(FadeInUIWithDelay());
        }

        StartCoroutine(LoadSceneAfterDelay());
    }

    private System.Collections.IEnumerator FadeInUIWithDelay()
    {
        // Wait for the specified delay before starting the fade
        yield return new WaitForSeconds(fadeStartDelay);

        // Get the Image component of the UI element
        Image uiImage = uiToFade.GetComponent<Image>();

        if (uiImage == null)
        {
            Debug.LogError("No Image component found on the UI element.");
            yield break;
        }

        // Ensure the Image is set to full screen, adjust RectTransform anchors, width, and height
        RectTransform rectTransform = uiToFade.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Stretch the image to cover the entire screen
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
        }

        // Fade the UI element to full opacity
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Lerp from 0 (transparent) to 1 (opaque)
            float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            // Apply the alpha to the Image component
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, alphaValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is fully opaque
        uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 1f);
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        // Wait before loading the scene
        yield return new WaitForSeconds(sceneLoadDelay);

        // Load the scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not specified!");
        }
    }
}
