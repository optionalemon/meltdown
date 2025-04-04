using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Exiting application...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}