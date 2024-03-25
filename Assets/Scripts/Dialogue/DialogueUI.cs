using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private Animator backgroundAnimator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [field: SerializeField] public AnimationClip BackgroundShowClip { get; private set; }

    public void ShowDialogueText(string message, float duration)
    {
        StartCoroutine(ManageDialogueDuration(message, duration));
    }

    private IEnumerator ManageDialogueDuration(string message, float duration)
    { 
        backgroundAnimator.SetBool("showDialogue", true);

        yield return new WaitForSecondsRealtime(BackgroundShowClip.length);
        dialogueText.text = message;

        yield return new WaitForSeconds(duration);
        dialogueText.text = string.Empty;
        backgroundAnimator.SetBool("showDialogue", false);
    }
}