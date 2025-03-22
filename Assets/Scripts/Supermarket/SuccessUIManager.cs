using System.Collections;
using UnityEngine;

public class SuccessUIManager : MonoBehaviour
{
    // Singleton instance
    public static SuccessUIManager Instance { get; private set; }
    
    [SerializeField] private GameObject successUI;
    [SerializeField] private float displayDuration = 5f;
    
    private Coroutine displayCoroutine;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        // Optional: Keep this object across scene loads
        // DontDestroyOnLoad(gameObject);
        
        // Make sure the UI is not active at start
        if (successUI != null)
        {
            successUI.SetActive(false);
        }
    }

    /// <summary>
    /// Shows the Success UI for the specified duration (default 15 seconds)
    /// Can be called from any script using SuccessUIManager.Instance.ShowSuccessUI()
    /// </summary>
    public void ShowSuccessUI()
    {
        successUI.SetActive(true);
        // If we already have a running coroutine, stop it first
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        
        // Start a new display coroutine
        displayCoroutine = StartCoroutine(DisplaySuccessUICoroutine());
    }
    
    private IEnumerator DisplaySuccessUICoroutine()
    {
        // Show the UI
        successUI.SetActive(true);
        
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);
        
        // Hide the UI after duration has passed
        successUI.SetActive(false);
        
        // Clear the coroutine reference
        displayCoroutine = null;
    }
    
    /// <summary>
    /// Forcefully hide the Success UI if needed
    /// </summary>
    public void HideSuccessUI()
    {
        // If we have a running coroutine, stop it
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }
        
        // Hide the UI
        successUI.SetActive(false);
    }
}