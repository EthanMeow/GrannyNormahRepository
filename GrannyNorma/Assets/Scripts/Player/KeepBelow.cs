using UnityEngine;

public class KeepBelow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The object to follow

    [Header("Offset")]
    public float verticalOffset = -1f; // Distance below the target

    void Update()
    {
        if (target != null)
        {
            // Set the position below the target
            Vector3 newPosition = new Vector3(target.position.x, target.position.y + verticalOffset, target.position.z);
            transform.position = newPosition;
        }
    }
}
