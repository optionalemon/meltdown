using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PosterInteraction : MonoBehaviour
{

    public InputActionReference leftTriggerAction;
    public InputActionReference triggerAction;  // Link to input action for button press
    public Camera camera;
    public float raycastDistance = 10f;
    private Ray ray;

    public float hoverHapticIntensity = 0.2f; // Intensity of the haptic feedback on hover
    public float clickHapticIntensity = 0.8f; // Intensity of the haptic feedback on click
    public float duration = 0.1f; // Duration of the haptic feedback

    private XRController xrController; // Reference to the XR controller

    public GameObject overlay;

    public Button closeButton;
    void Start()
    {
        // Get the XR controller. You can modify this to use the correct controller (e.g., left or right)
        xrController = GetComponent<XRController>();
        if (xrController == null)
        {
            Debug.Log("No devices found");
        }

        if (overlay != null)
        {
            overlay.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Overlay Canvas is not assigned!");
        }

        // Assign the Close Button's OnClick event if it's set in the Inspector
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideOverlay);
        }
        else
        {
            Debug.LogWarning("Close Button is not assigned!");
        }
    }

    void Update()
    {
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));  // Ray from controller center

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Poster"))
            {
                // Trigger haptic feedback when the controller is hovering over the poster
                TriggerHapticFeedback(hoverHapticIntensity);

                // Check if the user pressed the trigger to interact
                if (triggerAction.action.triggered || leftTriggerAction.action.triggered)
                {
                    Debug.Log("Poster clicked!");

                    // Trigger stronger haptic feedback when clicked
                    TriggerHapticFeedback(clickHapticIntensity);
                    ShowOverlay();
                }
            }
        }
    }

    // Function to trigger haptic feedback
    private void TriggerHapticFeedback(float intensity)
    {
        if (xrController != null)
        {
            // Send haptic feedback to the controller
            xrController.SendHapticImpulse(intensity, duration);
        }
    }

    private void ShowOverlay()
    {
        if (overlay != null)
        {
            overlay.SetActive(true);
        }
    }

    // Function to hide overlay (call this on button click)
    public void HideOverlay()
    {
        if (overlay != null)
        {
            overlay.SetActive(false);
        }
    }


}
