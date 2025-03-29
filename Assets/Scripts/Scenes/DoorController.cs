using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    [SerializeField] private TeleportationType teleportType;
    [SerializeField] private GameObject ConfirmAlertDialog;
    [SerializeField] private ConfirmationModal confirmationModal;
    [SerializeField] private bool isDoorHovered = false;

    public InputActionReference triggerAction;

    public enum TeleportationType
    {
        Supermarket,
        FoodWasteRoom,
        TutorialRoom,
        DisasterRoom,
        EndScene
    }

    void Awake()
    {
        // Get reference to XR Simple Interactable
        if (interactable == null)
        {
            interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        if (confirmationModal != null)
        {
            confirmationModal.onConfirm.AddListener(TeleportToSelectedScene);
        }

        if (ConfirmAlertDialog != null)
        {
            ConfirmAlertDialog.SetActive(false);
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
        // Only check trigger action if the door is being hovered over
        if (isDoorHovered && triggerAction.action.triggered)
        {
            if (ConfirmAlertDialog != null)
            {
                ConfirmAlertDialog.SetActive(true);
            }
        }
    }

    private void TeleportToSelectedScene()
    {
        // Start coroutine for sound and teleportation
        StartCoroutine(PlaySoundAndTeleport());
    }

    private IEnumerator PlaySoundAndTeleport()
    {
        // First, play the door open sound
        Object.FindFirstObjectByType<SoundManager>().PlaySound(SoundType.DOOR_OPEN);

        // Optional: Wait a short time to let the sound play
        yield return new WaitForSeconds(0.5f);

        // Make sure SceneNavigator singleton exists
        if (SceneNavigator.Instance == null)
        {
            Debug.LogError("SceneNavigator instance not found!");
            yield break;
        }

        // Call the appropriate method based on selected teleport type
        switch (teleportType)
        {
            case TeleportationType.Supermarket:
                SceneNavigator.Instance.GoToSupermarket();
                break;
            case TeleportationType.FoodWasteRoom:
                SceneNavigator.Instance.GoToFoodWasteRoom();
                break;
            case TeleportationType.TutorialRoom:
                SceneNavigator.Instance.GoToTutorialRoom();
                break;
            case TeleportationType.DisasterRoom:
                SceneNavigator.Instance.GoToDisasterRoom(DisasterEventType.NONE);
                break;
            case TeleportationType.EndScene:
                SceneNavigator.Instance.GoToEndScene();
                break;
        }
    }
}