using System.Collections;
using UnityEngine;


public class Behaviour_Dialogue : MonoBehaviour, IBehaviour
{
    [SerializeField] private int dialogueId;
    [SerializeField] private float dialogueDuration = 3;

    //This delay starts from the moment the behaviour is triggered, and is not managed by the controller. 
    [SerializeField] private float dialogueDelay; 

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        DialogueController.Instance.StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        yield return new WaitForSecondsRealtime(dialogueDelay);
        DialogueController.Instance.PlayDialogue(dialogueId, dialogueDuration);
    }
}
