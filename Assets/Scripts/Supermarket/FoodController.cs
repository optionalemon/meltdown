using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FoodController : MonoBehaviour
{
    [Header("Food Properties")]
    public bool isCorrectFood;
    public FoodItem foodType;

    [Header("References")]
    public Transform shoppingCartTransform;
    public GameObject confettiPrefab;
    public string eventType;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Collider[] colliders;
    
    // Reference to the shopping list controller
    private static ShoppingListController shoppingList;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        
        // Find shopping list controller if we don't have it yet
        if (shoppingList == null)
        {
            shoppingList = FindObjectOfType<ShoppingListController>();
        }

        // Check current food status
        if (SceneNavigator.Instance != null)
        {
            FoodStatus status = SceneNavigator.Instance.GetFoodStatus(foodType);
            
            if (status == FoodStatus.RightChoiceChosen)
            {
                if (!isCorrectFood)
                {
                    MakeNonInteractable();
                }
                else
                {
                Destroy(gameObject);
                }
            }
            else if (status == FoodStatus.WrongChoiceChosen)
            {
                // If a wrong choice was made:
                // - If this is the correct food, keep it visible but make it non-interactable
                // - If this is an incorrect food too, hide it
                if (isCorrectFood)
                {
                    MakeNonInteractable();
                }
                else
                {
                    Destroy(gameObject);
                }
                return;
            }
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnSelectExit);
        }
    }

    public void MakeNonInteractable()
    {
        // Disable the grab interactable
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }
        
        // Make the rigidbody kinematic
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        // Disable colliders or set them to trigger
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        Vector3 foodPos = transform.position;
        Vector3 cartPos = shoppingCartTransform.position;

        float xThreshold = 0.5f;
        float zThreshold = 0.5f;

        bool isAboveCartXZ = Mathf.Abs(foodPos.x - cartPos.x) <= xThreshold &&
                             Mathf.Abs(foodPos.z - cartPos.z) <= zThreshold;

        if (isAboveCartXZ)
        {
            if (isCorrectFood)
            {
                StartCoroutine(HandleCorrectDropSequence());
            }
            else
            {
                HandleIncorrectDrop();
            }
        }
        else
        {
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private IEnumerator HandleCorrectDropSequence()
    {
        SoundManager.Instance.PlaySound(SoundType.CORRECT_ITEM_PLACED);
        GameObject confetti = Instantiate(confettiPrefab, transform.position, Quaternion.identity);

        // Mark this food with the correct choice status
        SceneNavigator.Instance?.SetFoodStatus(foodType, FoodStatus.RightChoiceChosen);
        
        // Immediately disable all incorrect food options
        DisableIncorrectFoodOption();

        yield return new WaitForSeconds(1.0f);
        Destroy(confetti, 1.0f);
        Destroy(gameObject);
    }
    
    private void DisableIncorrectFoodOption()
    {
        // Find all food controllers in the scene
        FoodController[] allFoodControllers = FindObjectsOfType<FoodController>();
        
        // Disable the food controller that are not correct
        foreach (FoodController foodController in allFoodControllers)
        {
            if (foodController.foodType == foodType && !foodController.isCorrectFood)
            {
                foodController.MakeNonInteractable();
            }
        }
    }

    private void HandleIncorrectDrop()
    {
        // Mark this food with the wrong choice status
        SceneNavigator.Instance?.SetFoodStatus(foodType, FoodStatus.WrongChoiceChosen);
        
        // Make all correct food items visible but non-interactable
        FoodController[] allFoodControllers = FindObjectsOfType<FoodController>();
        foreach (FoodController foodController in allFoodControllers)
        {
            if (foodController != this && foodController.isCorrectFood)
            {
                foodController.MakeNonInteractable();
            }
        }
        
        // Destroy this wrong food item
        Destroy(gameObject);
        
        // Go to disaster room
        SceneNavigator.Instance?.GoToDisasterRoom(eventType);
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        if (rb != null) rb.isKinematic = true;

        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startPos, originalPosition, smoothT);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (rb != null) rb.isKinematic = false;
    }
}