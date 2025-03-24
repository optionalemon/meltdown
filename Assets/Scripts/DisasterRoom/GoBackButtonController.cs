using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoBackButtonController : MonoBehaviour
{
    [SerializeField] private Canvas goBackButton;
    [SerializeField] private float delayTime = 10f;
    void Start()
    {
        if (goBackButton != null)
        {
            goBackButton.gameObject.SetActive(false);
        }

        StartCoroutine(ShowButtonAfterDelay());
    }

    private IEnumerator ShowButtonAfterDelay()
    {

        yield return new WaitForSeconds(delayTime);

        if (goBackButton != null)
        {
            goBackButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can leave this empty or use it for other logic if needed
    }
}
