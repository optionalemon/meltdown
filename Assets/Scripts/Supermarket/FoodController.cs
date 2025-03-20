using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FoodController : MonoBehaviour
{
    [Header("Food Properties")]
    public bool isCorrectFood;

    [Header("References")]
    public Transform shoppingCartTransform;
    public GameObject confettiPrefab;
    public string eventType;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Destroy food if already thrown before
        if (SceneNavigator.Instance != null && SceneNavigator.Instance.HasFoodBeenThrown(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnSelectExit);
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

        // Mark this food as thrown so it stays gone next time
        SceneNavigator.Instance?.MarkFoodAsThrown(gameObject.name);

        yield return new WaitForSeconds(1.0f);
        Destroy(confetti, 1.0f);
        Destroy(gameObject);
    }

    private void HandleIncorrectDrop()
    {
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
