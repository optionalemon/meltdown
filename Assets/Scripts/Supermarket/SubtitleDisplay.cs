using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private GameObject subtitlePanel;
    
    [System.Serializable]
    public class SubtitleLine
    {
        public string text;
        public float startTime;
        public float duration;
    }
    
    [SerializeField] private SubtitleLine[] subtitleLines;
    
    private void Awake()
    {
        // Initialize default subtitles if none are set
        if (subtitleLines == null || subtitleLines.Length == 0)
        {
            subtitleLines = new SubtitleLine[]
            {
                new SubtitleLine { text = "Attention all shoppers!", startTime = 0.0f, duration = 2.0f },
                new SubtitleLine { text = "Global food shortages have reached critical levels.", startTime = 2.0f, duration = 3.0f },
                new SubtitleLine { text = "Every wasted meal brings us closer to disaster.", startTime = 5.0f, duration = 3.0f },
                new SubtitleLine { text = "Your mission: Gather ingredients, cook a meal,", startTime = 8.0f, duration = 3.0f },
                new SubtitleLine { text = "and prove that a zero-waste future is possible.", startTime = 11.0f, duration = 3.0f },
                new SubtitleLine { text = "Choose wisely, or face the consequences.", startTime = 14.0f, duration = 3.0f }
            };
        }
        
        // Hide panel initially
        if (subtitlePanel)
            subtitlePanel.SetActive(false);
    }
    
    private void Start()
    {
        // Position in front of camera
        PositionInFrontOfCamera();
    }
    
private void PositionInFrontOfCamera()
{
    Camera mainCamera = Camera.main;
    if (mainCamera != null)
    {
        // Calculate forward and down vectors
        Vector3 forward = mainCamera.transform.forward;
        Vector3 down = -mainCamera.transform.up;
        
        // Position canvas forward and downward from the camera
        // The 2f is the forward distance, 0.5f is the downward offset (adjust as needed)
        transform.position = mainCamera.transform.position + forward * 2f + down * 0.5f;
        
        // Make the canvas face the camera
        transform.rotation = Quaternion.LookRotation(
            mainCamera.transform.position - transform.position);
    }
}
    
    public void ShowSubtitles()
    {
        StartCoroutine(DisplaySubtitles());
    }
    
    private IEnumerator DisplaySubtitles()
    {
        if (subtitlePanel == null || subtitleText == null)
        {
            Debug.LogError("Subtitle panel or text component not assigned!");
            yield break;
        }

        subtitlePanel.SetActive(true);
        
        foreach (SubtitleLine line in subtitleLines)
        {
            // Calculate wait time
            float waitTime = 0;
            if (System.Array.IndexOf(subtitleLines, line) > 0)
            {
                int prevIndex = System.Array.IndexOf(subtitleLines, line) - 1;
                waitTime = line.startTime - (subtitleLines[prevIndex].startTime + subtitleLines[prevIndex].duration);
                if (waitTime < 0) waitTime = 0;
            }
            else
            {
                waitTime = line.startTime;
            }
            
            // Wait until it's time to show this line
            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);
            
            // Show the subtitle
            subtitleText.text = line.text;
            
            // Wait for the duration
            yield return new WaitForSeconds(line.duration);
            
            // Clear the text
            subtitleText.text = "";
        }
        
        // Hide the panel when done
        subtitlePanel.SetActive(false);
    }
}