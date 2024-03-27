using System.Collections;
using UnityEngine;

public class Behaviour_PlayAudio : MonoBehaviour, IBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField, Range(0, 1)] private float pitch = 1;
    [SerializeField, Range(0, 1)] private float spatialBlend = 0;
    [SerializeField] private bool playOneShot = true;
    [SerializeField] private bool useGeneralSource = true;
    [SerializeField] private AudioSource source;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        AudioManager.Instance.StartCoroutine(PlayAudio());
    }

    private IEnumerator PlayAudio()
    {
        if(useGeneralSource) source = AudioManager.Instance.generalAudioSource;

        if (!source) Debug.Log($"There is no audio source in play audio behaviour on {gameObject.name}!");
        if(!clip) Debug.Log($"There is no audio clip in play audio behaviour on {gameObject.name}!");

        if (source && clip)
        {
            yield return new WaitForSecondsRealtime(delay);

            source.pitch = pitch;
            source.spatialBlend = spatialBlend;

            if(playOneShot)
            {
                source.volume = 1;
                source.PlayOneShot(clip, volume);
                ResetSourceParameters();
            }
            else
            {
                source.volume = volume;
                source.clip = clip;
                source.Play();
                yield return new WaitForSecondsRealtime(clip.length);
                ResetSourceParameters();
            }
        }
    }

    private void ResetSourceParameters()
    {
        source.volume = 1;
        source.pitch = 1;
        source.spatialBlend = 0;
    }
}