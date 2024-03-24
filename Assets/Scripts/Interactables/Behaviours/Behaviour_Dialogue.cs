using System.Collections;
using UnityEngine;


public class Behaviour_Dialogue : MonoBehaviour, IBehaviour
{
    [SerializeField] private int dialogueId;
    [SerializeField] private float dialogueDelay;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        DialogueController.Instance.StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        yield return new WaitForSecondsRealtime(dialogueDelay);
        DialogueController.Instance.PlayDialogue(dialogueId);
    }
}
