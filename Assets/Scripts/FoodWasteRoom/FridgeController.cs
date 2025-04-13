using UnityEngine;
using UnityEngine.XR;

public class FridgeController : MonoBehaviour
{
     public Animation fridgeAnimation; // Legacy Animation component
    public string openAnimation = "open";
    public string closeAnimation = "close";

    private bool isOpen = false;
    private bool triggerPreviouslyPressed = false;

    void Update()
    {
        // Check both hands
        bool triggerPressed = IsTriggerPressed(XRNode.LeftHand) || IsTriggerPressed(XRNode.RightHand);

        if (triggerPressed && !triggerPreviouslyPressed)
        {
            // Toggle animation
            if (isOpen)
            {
                fridgeAnimation.Play(closeAnimation);
                isOpen = false;
            }
            else
            {
                fridgeAnimation.Play(openAnimation);
                isOpen = true;
            }
        }

        triggerPreviouslyPressed = triggerPressed;
    }

    bool IsTriggerPressed(XRNode hand)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(hand);
        bool triggerValue;
        return device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue;
    }
}
