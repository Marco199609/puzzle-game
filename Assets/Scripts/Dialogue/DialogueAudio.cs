using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField, Range(0, 1)] private float pitch = 1;
    [SerializeField, Range(0, 1)] private float spatialBlend = 0;

    public void PlayDialogueClip(AudioClip clip)
    {
        AudioManager.Instance.generalAudioSource.pitch = pitch;
        AudioManager.Instance.generalAudioSource.spatialBlend = spatialBlend;
        AudioManager.Instance.generalAudioSource.PlayOneShot(clip, volume);
        AudioManager.Instance.generalAudioSource.pitch = 1;
        AudioManager.Instance.generalAudioSource.spatialBlend = 0;
    }
}