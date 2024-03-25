using System.Collections;
using UnityEngine;

public class Behaviour_Dialogue : MonoBehaviour, IBehaviour
{
    [SerializeField] private int dialogueId;
    [SerializeField] private float dialogueDuration = 3;
    [SerializeField] private bool useAudioDuration = true;
    [SerializeField] private bool playOnce = true;

    [SerializeField, Tooltip("Remember this delay starts when the behaviour is triggered, and is not managed by the controller.")] 
    private float dialogueDelay;

    private bool alreadyPlayed;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        if(playOnce && !alreadyPlayed || !playOnce) DialogueController.Instance.StartCoroutine(ShowDialogue());
        if(playOnce) alreadyPlayed = true;
    }

    private IEnumerator ShowDialogue()
    {
        yield return new WaitForSecondsRealtime(dialogueDelay);
        DialogueController.Instance.PlayDialogue(dialogueId, dialogueDuration, useAudioDuration);
    }
}