using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [field: SerializeField] public AudioSource generalAudioSource { get; private set; }

    public static AudioManager Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }
}