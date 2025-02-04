using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeInOnSceneLoad : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeInDuration = 1f; // Time to fade in
    public Image fadeImage; // Reference to an Image UI component that will be used to fade the screen

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("No fade image assigned! Please assign an Image component in the inspector.");
            return;
        }

        // Start the fade-in process after the scene loads
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // Ensure the fade image is visible for the fade effect
        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;

        // Start with the screen being black (full alpha)
        fadeImage.color = new Color(0, 0, 0, 1);

        // Gradually fade the screen from black to transparent
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeInDuration)); // Fade back to transparent
            fadeImage.color = new Color(0, 0, 0, alpha); // Update the alpha
            yield return null;
        }

        // After fading in, hide the fade image (optional)
        fadeImage.gameObject.SetActive(false);
    }

    public void TriggerFadeIn()
    {
        StartCoroutine(FadeIn());
    }
}
