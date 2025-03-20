using UnityEngine;

public class PosterOverlayManager : MonoBehaviour
{
    public GameObject[] posterOverlays;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideAllOverlays()
    {
        for (int i = 0; i < posterOverlays.Length; i++)
        {
            posterOverlays[i].SetActive(false);
        }
    }
}
