using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Serializable] private enum Language { english, spanish }

    [SerializeField] private Language language;
    [SerializeField] private TextAsset json;

    public DialogueData dialogueTexts;
    public static DialogueController Instance;

    private float timeBetweenDialogueLines = 1;
    private DialogueUI ui;
    private DialogueAudio _audio;
    private Coroutine manageDialogueCache;
    private List<(int, (float, bool))> dialogueCache = new List<(int, (float, bool))>(); //id, duration, override audio duration
    private List<(DialogueType, Vector2)> dialogueTypeAndBubblePositions = new List<(DialogueType, Vector2)>();

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        dialogueTexts = JsonConvert.DeserializeObject<DialogueData>(json.text);
        ui = GetComponent<DialogueUI>();
        _audio = GetComponent<DialogueAudio>();
    }

    public void PlayDialogue(int id, float duration, DialogueType type, Vector2 bubblePosition = new Vector2(), bool useAudioDuration = true)
    {
        dialogueCache.Add((id,(duration, useAudioDuration)));
        dialogueTypeAndBubblePositions.Add((type, bubblePosition));
        if (manageDialogueCache == null) manageDialogueCache = StartCoroutine(ManageDialogueCache());

        Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
    }

    private IEnumerator ManageDialogueCache()
    {
        while (dialogueCache.Count > 0)
        {
            string text;
            switch (language)
            {
                case Language.english:
                    dialogueTexts.English.TryGetValue(dialogueCache[0].Item1, out text);
                    break;
                case Language.spanish:
                    dialogueTexts.Spanish.TryGetValue(dialogueCache[0].Item1, out text);
                    break;
                default:
                    dialogueTexts.English.TryGetValue(dialogueCache[0].Item1, out text);
                    break;
            }

            var dialogueClip = (AudioClip) Resources.Load($"Dialogues/dialogue{dialogueCache[0].Item1}");
            if(dialogueClip) _audio.PlayDialogueClip(dialogueClip);
            var dialogueDuration = GetDialogueDuration(dialogueClip);
            ui.ShowDialogueText(text, dialogueDuration, dialogueTypeAndBubblePositions[0]);

            yield return new WaitForSecondsRealtime(dialogueDuration + timeBetweenDialogueLines);
            dialogueCache.RemoveAt(0);
            dialogueTypeAndBubblePositions.RemoveAt(0);
            Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
        }

        manageDialogueCache = null;
    }

    private float GetDialogueDuration(AudioClip clip = null)
    {
        if (clip && dialogueCache[0].Item2.Item2 == true && clip.length > dialogueCache[0].Item2.Item1 + ui.BackgroundShowClip.length)
        {
            return clip.length + timeBetweenDialogueLines;
        }
        else
        {
            return dialogueCache[0].Item2.Item1 + ui.BackgroundShowClip.length + timeBetweenDialogueLines;
        }
    }
}