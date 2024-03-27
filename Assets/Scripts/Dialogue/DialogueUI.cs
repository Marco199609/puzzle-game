using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private Animator backgroundAnimator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private SpeechBubble speechBubblePrefab;
    [SerializeField] private Transform dialogueContainer;
    [SerializeField] private Camera _camera;
    [field: SerializeField] public AnimationClip BackgroundShowClip { get; private set; }

    public void ShowDialogueText(string message, float duration, DialogueType type, Vector2 bubblePos)
    {
        StartCoroutine(ManageDialogueDuration(message, duration, type, bubblePos));
    }

    private IEnumerator ManageDialogueDuration(string message, float duration, DialogueType type, Vector2 bubblePos)
    {
        if (type == DialogueType.Thought || type == DialogueType.Speech)
        {
            Instantiate(speechBubblePrefab, _camera.WorldToScreenPoint(bubblePos), Quaternion.identity, dialogueContainer).Set(message, duration, type);
            yield return null;
        }
        else if(type == DialogueType.Other)
        {
            backgroundAnimator.SetBool("showDialogue", true);

            yield return new WaitForSecondsRealtime(BackgroundShowClip.length);
            dialogueText.text = message;

            yield return new WaitForSecondsRealtime(duration);
            dialogueText.text = string.Empty;
            backgroundAnimator.SetBool("showDialogue", false);
        }
    }
}

public enum DialogueType
{
    Thought,
    Speech,
    Other
}