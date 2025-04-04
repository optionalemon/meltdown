using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DelayButtonController : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private float delayTime = 5f;
    void Start()
    {
        if (Button != null)
        {
            Button.gameObject.SetActive(false);
        }

        StartCoroutine(ShowButtonAfterDelay());
    }

    private IEnumerator ShowButtonAfterDelay()
    {

        yield return new WaitForSeconds(delayTime);

        if (Button != null)
        {
            Button.gameObject.SetActive(true);
        }
    }
}
