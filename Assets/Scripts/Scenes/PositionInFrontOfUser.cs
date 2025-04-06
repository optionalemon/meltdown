using UnityEngine;

public class PositionInFrontOfUser : MonoBehaviour
{
    [Header("Positioning Settings")]
    [Tooltip("Distance from the camera")]
    public float distanceFromUser = 1.5f;
    
    [Tooltip("Vertical offset from camera center")]
    public float verticalOffset = 0.0f;
    
    [Tooltip("Should the object rotate to face the user?")]
    public bool faceUser = true;
    
    [Tooltip("Should the object be positioned only once on start?")]
    public bool positionOnlyOnce = true;
    
    private bool hasPositioned = false;
    private Transform cameraTransform;
    
    void Start()
    {
        // Find the VR camera
        cameraTransform = Camera.main.transform;
        
        // Position the object on start
        if (positionOnlyOnce)
        {
            PositionInFront();
            hasPositioned = true;
        }
    }
    
    void Update()
    {
        // If we want to continuously update the position, do it in Update
        if (!positionOnlyOnce && !hasPositioned)
        {
            PositionInFront();
        }
    }
    
    void PositionInFront()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("No main camera found. Make sure you have a camera tagged as 'MainCamera'");
            return;
        }
        
        // Get the forward direction of the camera
        Vector3 cameraForward = cameraTransform.forward;
        
        // Calculate the position in front of the camera
        Vector3 newPosition = cameraTransform.position + cameraForward * distanceFromUser;
        
        // Add vertical offset
        newPosition.y += verticalOffset;
        
        // Set the object's position
        transform.position = newPosition;
        
        // Optionally make the object face the user
        if (faceUser)
        {
            transform.LookAt(new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z));
        }
    }
}