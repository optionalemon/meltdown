using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneNavigator : MonoBehaviour
{
    // Scene references
    [SerializeField] private SceneReference supermarketScene;
    [SerializeField] private SceneReference kitchenScene;
    [SerializeField] private SceneReference tutorialRoomScene;
    [SerializeField] private SceneReference disasterRoomScene;

    // Singleton pattern to access the navigator from any scene
    private static SceneNavigator _instance;
    public static SceneNavigator Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // Implement singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Methods to navigate between scenes
    public void GoToSupermarket()
    {
        LoadScene(supermarketScene);
    }

    public void GoToKitchen()
    {
        LoadScene(kitchenScene);
    }

    public void GoToTutorialRoom()
    {
        LoadScene(tutorialRoomScene);
    }

    public void GoToDisasterRoom()
    {
        LoadScene(disasterRoomScene);
    }

    // Helper method to load a scene
    private void LoadScene(SceneReference sceneRef)
    {
        // Start an asynchronous scene load
        StartCoroutine(LoadSceneWithEvents(sceneRef));
    }

    private IEnumerator LoadSceneWithEvents(SceneReference sceneRef)
    {
        // Begin loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneRef.BuildIndex);
        
        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Scene is loaded, now check which scene it was and perform scene-specific actions
        if (sceneRef == supermarketScene)
        {
            // Play the supermarket announcement
            FindObjectOfType<SoundManager>().PlaySound(SoundType.SUPERMARKET_ANNOUCEMENT);
        }
    }
}