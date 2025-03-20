using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneNavigator : MonoBehaviour
{
    [SerializeField] private SceneReference supermarketScene;
    [SerializeField] private SceneReference kitchenScene;
    [SerializeField] private SceneReference tutorialRoomScene;
    [SerializeField] private SceneReference disasterRoomScene;
    [SerializeField] private GameObject subtitleCanvasPrefab;

    public static string DISASTER_EVENT_TYPE;

    private GameObject currentSubtitleCanvas;
    private SceneReference currentScene;

    private static SceneNavigator instance;
    public static SceneNavigator Instance => instance;

    // Persistent food state
    private static HashSet<string> thrownFoodNames = new HashSet<string>();
    private static bool isAnnouncementPlayed;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

    public void GoToDisasterRoom(string eventType)
    {
        DISASTER_EVENT_TYPE = eventType;
        LoadScene(disasterRoomScene);
    }

    public bool HasFoodBeenThrown(string foodName)
    {
        return thrownFoodNames.Contains(foodName);
    }

    public void MarkFoodAsThrown(string foodName)
    {
        thrownFoodNames.Add(foodName);
    }

    private void LoadScene(SceneReference sceneRef)
    {
        currentScene = sceneRef;
        StartCoroutine(LoadSceneWithEvents(sceneRef));
    }

    private IEnumerator LoadSceneWithEvents(SceneReference sceneRef)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneRef.BuildIndex);
        while (!asyncLoad.isDone) yield return null;

        if (sceneRef == supermarketScene)
        {
            SoundManager.Instance.StopBackgroundMusic(true);

            if (!isAnnouncementPlayed) {
                isAnnouncementPlayed = true;
                currentSubtitleCanvas = Instantiate(subtitleCanvasPrefab);

                var subtitleDisplay = currentSubtitleCanvas.GetComponent<SubtitleDisplay>();
                SoundManager.Instance.PlaySound(SoundType.SUPERMARKET_ANNOUCEMENT);
                subtitleDisplay?.ShowSubtitles();

                AudioClip announcementClip = SoundManager.Instance.GetSoundClip(SoundType.SUPERMARKET_ANNOUCEMENT);
                if (announcementClip != null)
                    yield return new WaitForSeconds(announcementClip.length + 0.5f);
            }
            SoundManager.Instance.PlayBackgroundMusic(SoundType.SUPERMARKET_MUSIC, true);
        }
        else if (sceneRef == disasterRoomScene)
        {
            SoundManager.Instance.StopBackgroundMusic(true);
            SoundManager.Instance.PlayBackgroundMusic(SoundType.DISASTER_MUSIC, true);
        }
        else
        {
            if (currentSubtitleCanvas != null)
            {
                Destroy(currentSubtitleCanvas);
                currentSubtitleCanvas = null;
            }

            SoundManager.Instance?.StopBackgroundMusic(true);
        }
    }
}
