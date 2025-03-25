using UnityEngine;


public class GrabController : MonoBehaviour
{
    public float grabDistance = 1.2f; // Set your desired range
    public Transform leftHand; 
    public Transform rightHand;    // Assign in Inspector or auto-detect
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (leftHand == null)
            leftHand = GameObject.Find("LeftHand Controller")?.transform;
        if (rightHand == null)
            rightHand = GameObject.Find("RightHand Controller")?.transform;
    }

    void Update()
    {
        if (grabInteractable == null || leftHand == null || rightHand == null) return;

        float leftDist = Vector3.Distance(transform.position, leftHand.position);
        float rightDist = Vector3.Distance(transform.position, rightHand.position);

        bool isClose = leftDist <= grabDistance || rightDist <= grabDistance;
        grabInteractable.enabled = isClose;

    }
}
