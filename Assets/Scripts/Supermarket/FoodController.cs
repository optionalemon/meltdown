using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Linq;

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

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnSelectExit);
        }
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        float distanceToCart = Vector3.Distance(transform.position, shoppingCartTransform.position);
        float dropThreshold = 1.0f;

        if (distanceToCart <= dropThreshold)
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
        // Play sound
        SoundManager.Instance.PlaySound(SoundType.CORRECT_ITEM_PLACED);

        // Spawn confetti AFTER food disappears
        GameObject confetti = Instantiate(confettiPrefab, transform.position, Quaternion.identity);

        // Optional small delay to sync sound and visuals
        yield return new WaitForSeconds(1.0f);

        Destroy(confetti, 1.0f);

        // Hide food
        Destroy(gameObject);

    }

    private void HandleIncorrectDrop()
    {
        if (SceneNavigator.Instance != null)
        {
            SceneNavigator.Instance.GoToDisasterRoom(eventType);
        }
        else
        {
            Debug.LogWarning("SceneNavigator not found!");
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        if (rb != null)
            rb.isKinematic = true;

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

        if (rb != null)
            rb.isKinematic = false;
    }
}
