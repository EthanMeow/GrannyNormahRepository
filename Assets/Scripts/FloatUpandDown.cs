using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;          // Speed of the up-down movement
    public float height = 3f;         // Maximum height it will move up and down
    public float startHeight = 0f;    // Starting height of the object (you can change this in the Inspector)

    private Vector3 startPosition;    // To store the initial position of the object

    void Start()
    {
        // Store the starting position of the object
        startPosition = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    void Update()
    {
        // Move the object up and down using Mathf.Sin to create oscillating motion
        float newY = Mathf.Sin(Time.time * speed) * height + startPosition.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
