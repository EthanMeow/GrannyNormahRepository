using UnityEngine;

public class UIObjectSwitcher : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject objectToDeactivate; // The GameObject to turn off (along with its children)
    public GameObject objectToActivate;   // The GameObject to turn on (along with its children)

    /// <summary>
    /// Call this function to switch the active state of the objects and their children.
    /// </summary>
    public void SwitchObjects()
    {
        if (objectToDeactivate != null)
        {
            SetActiveState(objectToDeactivate, false); // Turn off object and its children
        }

        if (objectToActivate != null)
        {
            SetActiveState(objectToActivate, true); // Turn on object and its children
        }
    }

    /// <summary>
    /// Sets the active state of the specified GameObject and all its children.
    /// </summary>
    /// <param name="targetObject">The GameObject to update.</param>
    /// <param name="state">The active state to set (true to activate, false to deactivate).</param>
    private void SetActiveState(GameObject targetObject, bool state)
    {
        targetObject.SetActive(state);
    }
}
