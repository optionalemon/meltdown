using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneNavigator : MonoBehaviour {
    // Scene references
    [SerializeField] private SceneReference supermarketScene;
    [SerializeField] private SceneReference kitchenScene;
    [SerializeField] private SceneReference tutorialRoomScene;
    [SerializeField] private SceneReference disasterRoomScene;
    [SerializeField] private GameObject subtitleCanvasPrefab;
    
    private GameObject currentSubtitleCanvas;
    
    // Singleton pattern to access the navigator from any scene
    private static SceneNavigator instance;
    
    // Track current scene to handle music transitions properly
    private SceneReference currentScene;
    
    public static SceneNavigator Instance {
        get { return instance; }
    }
    
    private void Awake() {
        // Implement singleton pattern
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Methods to navigate between scenes
    public void GoToSupermarket() {
        LoadScene(supermarketScene);
    }
    
    public void GoToKitchen() {
        LoadScene(kitchenScene);
    }
    
    public void GoToTutorialRoom() {
        LoadScene(tutorialRoomScene);
    }
    
    public void GoToDisasterRoom() {
        LoadScene(disasterRoomScene);
    }
    
    // Helper method to load a scene
    private void LoadScene(SceneReference sceneRef) {
        // Update current scene
        currentScene = sceneRef;
        
        // Start an asynchronous scene load
        StartCoroutine(LoadSceneWithEvents(sceneRef));
    }
    
    private IEnumerator LoadSceneWithEvents(SceneReference sceneRef) {
        // Begin loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneRef.BuildIndex);
        
        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone) {
            yield return null;
        }
        
        // Scene is loaded, now check which scene it was and perform scene-specific actions
        if (sceneRef == supermarketScene) {
            currentSubtitleCanvas = Instantiate(subtitleCanvasPrefab);
            
            // Get reference to the subtitle component on the canvas
            SubtitleDisplay subtitleDisplay = currentSubtitleCanvas.GetComponent<SubtitleDisplay>();
            
            // Play the supermarket announcement
            SoundManager.Instance.PlaySound(SoundType.SUPERMARKET_ANNOUCEMENT);
            
            if (subtitleDisplay != null) {
                subtitleDisplay.ShowSubtitles();
            }
        
            // Wait for the announcement to finish before starting background music
            AudioClip announcementClip = SoundManager.Instance.GetSoundClip(SoundType.SUPERMARKET_ANNOUCEMENT);
            if (announcementClip != null) {
                yield return new WaitForSeconds(announcementClip.length + 0.5f); // Add a small delay
            }
            
            // Now start the background music with fade in
            SoundManager.Instance.PlayBackgroundMusic(SoundType.SUPERMARKET_MUSIC, true);
        }
        else {
            // Clean up subtitle canvas if we're leaving the supermarket
            if (currentSubtitleCanvas != null) {
                Destroy(currentSubtitleCanvas);
                currentSubtitleCanvas = null;
            }
            
            // Stop background music if it's playing
            if (SoundManager.Instance != null) {
                SoundManager.Instance.StopBackgroundMusic(true);
            }
        }
    }
}