using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : MonoBehaviour
{
    public ParticleSystem scanningParticles;
    public float scanDistance = 1.0f;
    public float minScanDistance = 0.05f; // Minimum distance to prevent "too close" issues
    public float scanConeAngle = 30f; // Cone angle for wider scanning area
    public LayerMask foodLayer;
    public Transform scanOrigin; // Optional: A specific point where the scan originates
    
    private GameObject currentTarget;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Awake()
    {
        // Store the original position and rotation when the object is created
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        
        // If no scan origin is assigned, use this transform
        if (scanOrigin == null)
            scanOrigin = transform;
    }
    
    void Start()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable != null)
        {
            // Add listeners for selection events
            interactable.selectEntered.AddListener(OnSelectEnter);
            interactable.selectExited.AddListener(OnSelectExit);
            
            // Keep the activation listeners for scanning functionality
            var grabInteractable = interactable as UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable;
            if (grabInteractable != null)
            {
                grabInteractable.activated.AddListener(x => StartScanning());
                grabInteractable.deactivated.AddListener(x => StopScanning());
            }
        }
    }
    
    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Get the interactor from the args
        var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (interactor != null)
        {
            // For newer XR Interaction Toolkit versions
            if (interactor.GetType().GetProperty("selectActionTrigger") != null)
            {
                // Use reflection to set the property if it exists
                interactor.GetType().GetProperty("selectActionTrigger").SetValue(interactor, 3);
            }
            
            // For poke interactors or other types
            if (interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor pokeInteractor)
            {
                // Set appropriate properties for the poke interactor to make it sticky
                var field = pokeInteractor.GetType().GetField("m_SelectActionTrigger", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(pokeInteractor, 3); 
                }
            }
        }
    }
    
    private void OnSelectExit(SelectExitEventArgs args)
    {
        // Get the interactor from the args
        var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (interactor != null)
        {
            // For newer XR Interaction Toolkit versions
            if (interactor.GetType().GetProperty("selectActionTrigger") != null)
            {
                // Use reflection to set the property if it exists
                interactor.GetType().GetProperty("selectActionTrigger").SetValue(interactor, 0); // 0 = StateChange in enum
            }
            
            // For poke interactors or other types
            if (interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor pokeInteractor)
            {
                // Reset appropriate properties for the poke interactor
                var field = pokeInteractor.GetType().GetField("m_SelectActionTrigger", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(pokeInteractor, 0); // 0 = StateChange in enum
                }
            }
        }
        StartCoroutine(ReturnToPosition());
    }

    private IEnumerator ReturnToPosition()
    {
        // Make sure any physics interactions are disabled during the return
        Rigidbody rb = GetComponent<Rigidbody>();
        bool wasKinematic = false;
        
        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
            rb.isKinematic = true;
        }
        
        // Animate the return over a short time
        float duration = 0.3f;
        float elapsed = 0;
        
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Use smoothstep or other easing function for smoother motion
            float smoothT = t * t * (3f - 2f * t);
            
            transform.position = Vector3.Lerp(startPosition, originalPosition, smoothT);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, smoothT);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure it ends exactly at the original position
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        
        // Restore the original rigidbody settings
        if (rb != null)
        {
            rb.isKinematic = wasKinematic;
        }
    }
    
    void Update()
    {
        // Use a spherecast for more forgiving detection
        if (ScanForFood())
        {
            // Target is already set in ScanForFood()
        }
        else
        {
            if (currentTarget != null)
            {
                StopScanning();
            }
            currentTarget = null;
        }
    }

    // Get the actual scan direction (90 degrees to the left of forward)
    Vector3 GetScanDirection()
    {
        // This rotates the forward vector 90 degrees to the left around the up axis
        return Quaternion.Euler(0, -90, 0) * scanOrigin.forward;
    }
    
    bool ScanForFood()
    {
        // Get the scan direction (90 degrees to the left)
        Vector3 scanDirection = GetScanDirection();
        
        // First check if anything is too close (within minScanDistance)
        Collider[] closeColliders = Physics.OverlapSphere(scanOrigin.position, minScanDistance, foodLayer);
        if (closeColliders.Length > 0)
        {
            // Something is too close, handle it as a valid target
            GameObject newTarget = closeColliders[0].gameObject;
            if (currentTarget != newTarget)
            {
                if (currentTarget != null)
                {
                    StopScanning();
                }
                currentTarget = newTarget;
            }
            return true;
        }
        
        // Try a spherecast for more forgiving detection
        float sphereRadius = 0.05f; // Adjust based on your needs
        RaycastHit[] hits = Physics.SphereCastAll(scanOrigin.position, sphereRadius, scanDirection, scanDistance, foodLayer);
        
        // If spherecast found nothing, try multiple raycasts in a cone pattern for wider coverage
        if (hits.Length == 0)
        {
            return TryConeScan();
        }
        
        // Process spherecast hits
        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestTarget = hit.collider.gameObject;
            }
        }
        
        if (closestTarget != null)
        {
            if (currentTarget != closestTarget)
            {
                if (currentTarget != null)
                {
                    StopScanning();
                }
                currentTarget = closestTarget;
            }
            return true;
        }
        
        return false;
    }
    
    bool TryConeScan()
    {
        // The base scan direction is 90 degrees left of forward
        Vector3 scanDirection = GetScanDirection();
        
        // Create a new coordinate system for the cone, with scanDirection as forward
        Vector3 forward = scanDirection;
        Vector3 up = scanOrigin.up;
        Vector3 right = Vector3.Cross(forward, up).normalized;
        
        // Recalculate up to ensure orthogonality
        up = Vector3.Cross(right, forward).normalized;
        
        // Center ray
        if (TryRaycast(forward))
            return true;
            
        // Angled rays
        float angle = scanConeAngle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        
        // Try rays at different angles around the scan direction
        if (TryRaycast(forward * cos + up * sin))
            return true;
        if (TryRaycast(forward * cos - up * sin))
            return true;
        if (TryRaycast(forward * cos + right * sin))
            return true;
        if (TryRaycast(forward * cos - right * sin))
            return true;
            
        return false;
    }
    
    bool TryRaycast(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(scanOrigin.position, direction, out hit, scanDistance, foodLayer))
        {
            if (currentTarget != hit.collider.gameObject)
            {
                if (currentTarget != null)
                {
                    StopScanning();
                }
            }
            currentTarget = hit.collider.gameObject;
            return true;
        }
        return false;
    }

    public void StartScanning()
    {
        scanningParticles.Play();
        
        // If we have a valid food item target
        if (currentTarget != null)
        {
            // Try to get ScanableFood component from hit object or its parents
            ScanableFood scanableFood = currentTarget.GetComponent<ScanableFood>();
            if (scanableFood == null)
            {
                scanableFood = currentTarget.GetComponentInParent<ScanableFood>();
            }
            
            // If we found a scanable food, tell it to start scanning
            if (scanableFood != null)
            {
                scanableFood.StartScan();
            }
        }
    }

    public void StopScanning()
    {
        scanningParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        // If we have a valid food item target
        if (currentTarget != null)
        {
            // Try to get ScanableFood component from hit object or its parents
            ScanableFood scanableFood = currentTarget.GetComponent<ScanableFood>();
            if (scanableFood == null)
            {
                scanableFood = currentTarget.GetComponentInParent<ScanableFood>();
            }
            
            // If we found a scanable food, tell it to start scanning
            if (scanableFood != null)
            {
                scanableFood.StopScan();
            }
        }
    }
    
    // Optional: Visualize the scan area in the editor
    void OnDrawGizmosSelected()
    {
        if (scanOrigin == null)
            scanOrigin = transform;
            
        // Get the scan direction (90 degrees to the left)
        Vector3 scanDirection = Quaternion.Euler(0, -90, 0) * scanOrigin.forward;
        
        // Draw the main scan ray
        Gizmos.color = Color.green;
        Gizmos.DrawRay(scanOrigin.position, scanDirection * scanDistance);
        
        // Draw the cone
        Gizmos.color = Color.yellow;
        
        // Create a new coordinate system for the cone
        Vector3 forward = scanDirection;
        Vector3 up = scanOrigin.up;
        Vector3 right = Vector3.Cross(forward, up).normalized;
        up = Vector3.Cross(right, forward).normalized;
        
        float angle = scanConeAngle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        
        Gizmos.DrawRay(scanOrigin.position, (forward * cos + up * sin) * scanDistance);
        Gizmos.DrawRay(scanOrigin.position, (forward * cos - up * sin) * scanDistance);
        Gizmos.DrawRay(scanOrigin.position, (forward * cos + right * sin) * scanDistance);
        Gizmos.DrawRay(scanOrigin.position, (forward * cos - right * sin) * scanDistance);
        
        // Draw the minimum distance sphere
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(scanOrigin.position, minScanDistance);
    }
}