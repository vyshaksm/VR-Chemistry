using UnityEngine;
using UnityEngine.InputSystem;

public class VRToggleObject : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToToggle;
    public InputActionProperty toggleAction;

    // Enables the configured input action when this component becomes active.
    void OnEnable()
    {
        toggleAction.action.Enable();
    }

    // Checks for the toggle input each frame.
    void Update()
    {
        if (toggleAction.action.WasPressedThisFrame())
        {
            Toggle();
        }
    }

    // Flips the target object's active state.
    private void Toggle()
    {
        if (objectToToggle != null)
        {
            bool currentState = objectToToggle.activeSelf;
            objectToToggle.SetActive(!currentState);
            Debug.Log($"Toggled {objectToToggle.name} to {!currentState}");
        }
    }
}
