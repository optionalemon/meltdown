using UnityEngine;

public enum SoundType {
    DOOR_OPEN,
    SUPERMARKET_ANNOUCEMENT,
    SUPERMARKET_MUSIC,
    BUTTON_CLICK
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType soundType, float volume = 1f)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)soundType], volume);
    }
}
