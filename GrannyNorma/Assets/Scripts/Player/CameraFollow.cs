using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;  // The object that the camera follows (the player)

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0, 2, -10);  // Offset from the target position
    public float smoothSpeed = 0.125f;  // Speed of smoothing the camera movement

    [Header("Zoom Settings")]
    public bool isOrthographic = true;  // Is the camera orthographic (2D)?
    public float zoomSpeed = 2f;        // Speed of zooming
    public float zoomedInSize = 3f;     // Orthographic size or field of view when zoomed in
    public float zoomDuration = 2f;     // Duration of the zoom effect
    public float yOffsetDecrease = 2f;  // Amount to move the camera down (in Y) during zoom

    private Camera cam;                 // Reference to the Camera component
    private bool isZooming = false;     // Tracks if the camera is currently zooming
    private float defaultSize;          // Default orthographic size or field of view
    private float defaultYOffset;       // The original Y offset before zooming in

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("No Camera component found on this GameObject!");
            return;
        }

        // Store the default size and Y offset
        defaultSize = isOrthographic ? cam.orthographicSize : cam.fieldOfView;
        defaultYOffset = offset.y;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position with the updated offset
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate between current and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the new position to the camera
            transform.position = smoothedPosition;
        }
    }

    public void ZoomIn()
    {
        if (!isZooming)
        {
            StartCoroutine(SmoothZoom(zoomedInSize, new Vector3(offset.x, offset.y - yOffsetDecrease, offset.z)));
        }
    }

    public void ZoomOut()
    {
        if (!isZooming)
        {
            StartCoroutine(SmoothZoom(defaultSize, new Vector3(offset.x, defaultYOffset, offset.z)));
        }
    }

    private IEnumerator SmoothZoom(float targetSize, Vector3 targetOffset)
    {
        isZooming = true;
        float elapsedTime = 0f;

        // Get the current size of the camera
        float initialSize = isOrthographic ? cam.orthographicSize : cam.fieldOfView;
        Vector3 initialOffset = offset;

        while (elapsedTime < zoomDuration)
        {
            // Smoothly interpolate the size and offset
            float newSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / zoomDuration);
            Vector3 newOffset = Vector3.Lerp(initialOffset, targetOffset, elapsedTime / zoomDuration);

            if (isOrthographic)
            {
                cam.orthographicSize = newSize;
            }
            else
            {
                cam.fieldOfView = newSize;
            }

            offset = newOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final size and offset are applied
        if (isOrthographic)
        {
            cam.orthographicSize = targetSize;
        }
        else
        {
            cam.fieldOfView = targetSize;
        }

        offset = targetOffset;

        isZooming = false;
    }
}
