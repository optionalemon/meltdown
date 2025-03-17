using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Linq;

public class FoodController : MonoBehaviour
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject confirmAlertDialog;
    [SerializeField] private ConfirmationModal confirmationModal;
    [SerializeField] private Transform shoppingCartTransform;
    [SerializeField] private string[] correctFoodNames; // List of correct food item names (or tags)
    [SerializeField] private Animator correctChoiceAnimator;
    [SerializeField] private AudioSource correctChoiceSound;

    public InputActionReference gripAction;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isConfirmed = false;

    void Awake()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        // Disable grab initially
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
            grabInteractable.selectExited.AddListener(OnDropped);
        }

        // Store original shelf position
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Setup confirmation
        if (confirmationModal != null)
        {
            confirmationModal.onConfirm.AddListener(OnUserConfirmedGrab);
        }

        if (confirmAlertDialog != null)
        {
            confirmAlertDialog.SetActive(false);
        }
    }

    void Update()
    {
        if (gripAction != null && gripAction.action.WasPressedThisFrame() && !isConfirmed)
        {
            ShowConfirmationDialog();
        }
    }

    private void ShowConfirmationDialog()
    {
        if (confirmAlertDialog != null)
        {
            confirmAlertDialog.SetActive(true);
        }
    }

    private void OnUserConfirmedGrab()
    {
        isConfirmed = true;
        grabInteractable.enabled = true;
        if (confirmAlertDialog != null)
        {
            confirmAlertDialog.SetActive(false);
        }
    }

    private void OnDropped(SelectExitEventArgs args)
    {
        if (IsInCart())
        {
            HandleDropInCart();
        }
        else
        {
            ReturnToShelf();
        }
    }

    private bool IsInCart()
    {
        float distance = Vector3.Distance(transform.position, shoppingCartTransform.position);
        return distance < 1.0f; // You can tweak this radius as needed
    }

    private void HandleDropInCart()
    {
        bool isCorrect = false;
        foreach (string foodName in correctFoodNames)
        {
            if (gameObject.name.Contains(foodName))
            {
                isCorrect = true;
                break;
            }
        }

        if (isCorrect)
        {
            StartCoroutine(PlayCorrectChoiceFeedback());
        }
        else
        {
            GoToDisasterRoom();
        }
    }

    private IEnumerator PlayCorrectChoiceFeedback()
    {
        if (correctChoiceAnimator != null)
        {
            correctChoiceAnimator.SetTrigger("Play");
        }

        if (correctChoiceSound != null)
        {
            correctChoiceSound.Play();
        }

        yield return new WaitForSeconds(1.0f); // Allow animation/sound to play before removing

        Destroy(gameObject);
    }

    private void ReturnToShelf()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);

        // Force deselect using first selecting interactor
        if (grabInteractable.isSelected)
        {
            var interactor = grabInteractable.interactorsSelecting.FirstOrDefault();
            if (interactor != null && grabInteractable.interactionManager != null)
            {
                grabInteractable.interactionManager.SelectExit(
                    interactor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor,
                    grabInteractable as UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable
                );
            }
        }
    }

    private void GoToDisasterRoom()
    {
        if (SceneNavigator.Instance != null)
        {
            SceneNavigator.Instance.GoToDisasterRoom();
        }
        else
        {
            Debug.LogWarning("SceneNavigator not found!");
        }
    }
}
