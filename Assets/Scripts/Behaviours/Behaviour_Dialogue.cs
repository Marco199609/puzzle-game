using System.Collections;
using UnityEngine;

public class Behaviour_Dialogue : MonoBehaviour, IBehaviour
{
    [Header("If speech or dialogue, place child named BubbleSpawn")]
    [SerializeField] private DialogueType type;
    [SerializeField] private int dialogueId;
    [SerializeField] private float dialogueDuration = 3;
    [SerializeField] private bool useAudioDuration = true;
    [SerializeField] private bool playOnce = true;

    [SerializeField, Tooltip("Remember this delay starts when the behaviour is triggered, and is not managed by the controller.")]
    private float dialogueDelay;

    private bool alreadyPlayed;
    private Transform speechBubbleSpawnPoint;

    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        GetBubbleSpawner();

        if(playOnce && !alreadyPlayed || !playOnce) DialogueController.Instance.StartCoroutine(ShowDialogue());
        if(playOnce) alreadyPlayed = true;
    }

    private void GetBubbleSpawner()
    {
        switch(type)
        {
            case DialogueType.Speech:
            case DialogueType.Thought:
                foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (child.name == "BubbleSpawn") speechBubbleSpawnPoint = child;
                }

                speechBubbleSpawnPoint = !speechBubbleSpawnPoint ? transform : speechBubbleSpawnPoint;
                Debug.LogWarning($"Could not find speech bubble spawner on gameobject: {gameObject.name}, parent: {transform.parent.name}!");
                break;
            case DialogueType.Other:
                speechBubbleSpawnPoint = transform;
                break;
        }
    }

    private IEnumerator ShowDialogue()
    {
        yield return new WaitForSecondsRealtime(dialogueDelay);
        DialogueController.Instance.PlayDialogue(dialogueId, dialogueDuration, type, speechBubbleSpawnPoint.position, useAudioDuration);
    }
}