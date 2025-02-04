using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeScreenAndLoadSceneWithCollectibles : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeOutDuration = 1f; // Time to fade out
    public float fadeInDuration = 1f;  // Time to fade in after scene load
    public float sceneLoadDelay = 2f; // Delay before loading the scene after the fade out

    [Header("Scene Settings")]
    public string sceneToLoad; // The name of the scene to load

    [Header("UI Reference")]
    public Image fadeImage; // Reference to an Image UI component that will be used to fade the screen

    [Header("Player Reference")]
    public Transform player; // Reference to the player object

    [Header("Object Reference")]
    public Transform targetObject; // The object that triggers the fade

    public float triggerDistance = 3f; // The distance at which the player triggers the fade

    [Header("Objects to Disable")]
    public GameObject[] objectsToDisable; // Array of objects to disable when the player enters range

    [Header("Collectibles")]
    public int requiredCollectibles = 5; // The amount of collectibles required to trigger the scene load
    public GameManager gameManager; // Reference to GameManager to track the player's collected amount

    private bool hasTriggered = false;

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("No fade image assigned! Please assign an Image component in the inspector.");
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("No GameManager assigned! Please assign a GameManager reference in the inspector.");
            return;
        }

        // Hide fade image initially
        fadeImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player != null && targetObject != null && !hasTriggered)
        {
            // Check if the player is within a certain range of the target object
            float distance = Vector3.Distance(player.position, targetObject.position);

            if (distance <= triggerDistance && gameManager.collectedAmount >= requiredCollectibles)
            {
                hasTriggered = true; // Make sure the fade only triggers once

                // Disable the specified objects
                DisableObjects();

                // Start the fade and load sequence
                StartCoroutine(FadeOutAndLoadScene());
            }
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Disabled: {obj.name}");
            }
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        // Start the fade-out effect
        yield return StartCoroutine(FadeOut());

        // Wait before loading the scene
        yield return new WaitForSeconds(sceneLoadDelay);

        // Subtract the required amount of collectibles from the player
        gameManager.AddToCollected(-requiredCollectibles);

        // Load the new scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not specified!");
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        // Show the fade image and start fading
        fadeImage.gameObject.SetActive(true);

        // Gradually fade the screen to black
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            fadeImage.color = new Color(0, 0, 0, alpha); // Fade to black
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        // Gradually fade the screen from black to transparent
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeInDuration));
            fadeImage.color = new Color(0, 0, 0, alpha); // Fade back to transparent
            yield return null;
        }

        // After fading in, hide the fade image
        fadeImage.gameObject.SetActive(false);
    }
}
