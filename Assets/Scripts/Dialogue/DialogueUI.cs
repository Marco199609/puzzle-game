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

    public void ShowDialogueText(string message, float duration, (DialogueType, Vector2) typeAndBubblePos)
    {
        StartCoroutine(ManageDialogueDuration(message, duration, typeAndBubblePos));
    }

    private IEnumerator ManageDialogueDuration(string message, float duration, (DialogueType, Vector2) typeAndBubblePos)
    {
        if (typeAndBubblePos.Item1 == DialogueType.Thought || typeAndBubblePos.Item1 == DialogueType.Speech)
        {
            Instantiate(speechBubblePrefab, _camera.WorldToScreenPoint(typeAndBubblePos.Item2), Quaternion.identity, dialogueContainer).Set(message, duration, typeAndBubblePos.Item1);
            yield return null;
        }
        else if(typeAndBubblePos.Item1 == DialogueType.Other)
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