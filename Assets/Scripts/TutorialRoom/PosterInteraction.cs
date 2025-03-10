using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR;
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
    public float duration = 0.00001f; // Duration of the haptic feedback

    private UnityEngine.XR.InputDevice leftControllerDevice;
    private UnityEngine.XR.InputDevice rightControllerDevice;

    public GameObject overlay;

    public Button closeButton;

    public GameObject highlightObject;

    private bool isHovering = false;
    void Start()
    {
        // Get the XR controller. You can modify this to use the correct controller (e.g., left or right)
        InitializeControllers();

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

        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Highlight Object is not assigned!");
        }
    }

    private void HighlightPoster(bool isHovered)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(isHovered);
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


                if (!isHovering)
                {
                    HighlightPoster(true);
                    isHovering = true;
                }

                // Check if the user pressed the trigger to interact
                if (triggerAction.action.triggered || leftTriggerAction.action.triggered)
                {
                    Debug.Log("Poster clicked!");

                    // Trigger stronger haptic feedback when clicked
                    // if (triggerAction.action.triggered)
                    // {
                    //     TriggerHapticFeedback(clickHapticIntensity, "left");

                    // }
                    // else
                    // {
                    //     TriggerHapticFeedback(clickHapticIntensity, "right");
                    // }
                    ShowOverlay();
                }
            }
            else
            {
                // If not hovering over the poster, remove the highlight
                if (isHovering)
                {
                    HighlightPoster(false);
                    isHovering = false;
                }
            }
        }
    }

    // Function to trigger haptic feedback
    private void TriggerHapticFeedback(float intensity, string controller)
    {
        if (leftControllerDevice.isValid && controller == "left")
        {
            leftControllerDevice.SendHapticImpulse(0, intensity, duration);
        }

        // Check if right controller is valid and send haptic feedback
        if (rightControllerDevice.isValid && controller == "right")
        {
            rightControllerDevice.SendHapticImpulse(0, intensity, duration);
        }
    }

    private void InitializeControllers()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

        // Get the Left Hand controller device
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
        {
            leftControllerDevice = devices[0];
            Debug.Log("Left Controller Initialized: " + leftControllerDevice.name);
        }
        else
        {
            Debug.LogWarning("Left Controller not found.");
        }

        // Get the Right Hand controller device
        devices.Clear();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightControllerDevice = devices[0];
            Debug.Log("Right Controller Initialized: " + rightControllerDevice.name);
        }
        else
        {
            Debug.LogWarning("Right Controller not found.");
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
