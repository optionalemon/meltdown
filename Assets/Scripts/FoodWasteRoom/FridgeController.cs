using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class FridgeController : MonoBehaviour
{
     public Animation fridgeAnimation; // Legacy Animation component
    public string openAnimation = "open";
    public string closeAnimation = "close";
    [SerializeField] private XRBaseInteractable interactable;

    public InputActionReference triggerAction;

    private bool isOpen = false;

    private bool isDoorHovered = false;

        void Awake()
    {
        // Get reference to XR Simple Interactable
        if (interactable == null)
        {
            interactable = GetComponent<XRGrabInteractable>();
        }
        
        // Add hover events to the interactable
        if (interactable != null && isDoorHovered == false)
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.hoverExited.AddListener(OnHoverExit);
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        isDoorHovered = true;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        isDoorHovered = false;
    }


    void Update()
    {
       if (isDoorHovered && triggerAction.action.triggered)
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
    }
}
