using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Serializable] private enum Language { english, spanish }

    [SerializeField] private Language language;
    [SerializeField] private TextAsset json;

    public DialogueData dialogueTexts;
    public static DialogueController Instance;

    private float minimumTimeBetweenLines = 1;
    private DialogueUI ui;
    private DialogueAudio _audio;
    private Coroutine manageOtherDialogueCache;
    private List<(int, bool)> dialogueCache = new List<(int, bool)>(); //id, use audio duration
    private List<float> dialogueDefaultDurations = new List<float> ();
    private List<(DialogueType, Vector2)> dialogueTypeAndBubblePositions = new List<(DialogueType, Vector2)>();

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        dialogueTexts = JsonConvert.DeserializeObject<DialogueData>(json.text);
        ui = GetComponent<DialogueUI>();
        _audio = GetComponent<DialogueAudio>();
    }

    public void PlayDialogue(int id, float defaultDuration, DialogueType type, Vector2 bubblePosition = new Vector2(), bool useAudioDuration = true)
    {
        switch(type)
        {
            case DialogueType.Speech:
            case DialogueType.Thought:
                SetDialogue(id, defaultDuration, useAudioDuration, type, bubblePosition, out float calculatedDuration);
                break;
            case DialogueType.Other:
                AddToDialogueCaches(id, useAudioDuration, defaultDuration, type, bubblePosition);
                if (manageOtherDialogueCache == null) manageOtherDialogueCache = StartCoroutine(ManageOtherDialogueCache());
                Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
                break;
        }
    }

    private IEnumerator ManageOtherDialogueCache()
    {
        while (dialogueCache.Count > 0)
        {
            yield return new WaitUntil(() => !CinematicsController.Instance.CutsceneActive);

            SetDialogue(
                id: dialogueCache[0].Item1, 
                defaultDuration: dialogueDefaultDurations[0], 
                useAudioDuration: dialogueCache[0].Item2, 
                type: dialogueTypeAndBubblePositions[0].Item1, 
                bubblePos: dialogueTypeAndBubblePositions[0].Item2, 
                out float calculatedDuration);

            yield return new WaitForSecondsRealtime(calculatedDuration + minimumTimeBetweenLines);
            RemoveFromDialogueCaches(index: 0);
            Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
        }

        manageOtherDialogueCache = null;
    }

    private void SetDialogue(int id, float defaultDuration, bool useAudioDuration, DialogueType type, Vector2 bubblePos, out float calculatedDuration)
    {
        PlayDialogueClip(id, out float clipDuration);
        calculatedDuration = GetDialogueDuration(clipDuration, useAudioDuration, defaultDuration);
        ui.ShowDialogueText(GetDialogueText(id), calculatedDuration, type, bubblePos);
    }

    private void AddToDialogueCaches(int id, bool useAudioDuration, float defaultDuration, DialogueType type, Vector2 bubblePosition)
    {
        dialogueCache.Add((id, useAudioDuration));
        dialogueDefaultDurations.Add(defaultDuration);
        dialogueTypeAndBubblePositions.Add((type, bubblePosition));
    }

    private void RemoveFromDialogueCaches(int index)
    {
        dialogueCache.RemoveAt(index);
        dialogueDefaultDurations.RemoveAt(index);
        dialogueTypeAndBubblePositions.RemoveAt(index);
    }

    private string GetDialogueText(int id)
    {
        string text;

        switch (language)
        {
            case Language.english:
                dialogueTexts.English.TryGetValue(id, out text);
                break;
            case Language.spanish:
                dialogueTexts.Spanish.TryGetValue(id, out text);
                break;
            default:
                dialogueTexts.English.TryGetValue(id, out text);
                break;
        }

        return text;
    }

    private void PlayDialogueClip(int id, out float clipDuration)
    {
        var dialogueClip = (AudioClip)Resources.Load($"Dialogues/dialogue{id}");
        if (dialogueClip) _audio.PlayDialogueClip(dialogueClip);
        clipDuration = dialogueClip ? dialogueClip.length : 0;
    }

    private float GetDialogueDuration(float clipDuration, bool useAudioDuration, float defaultDuration)
    {
        if (useAudioDuration && clipDuration > 0 && clipDuration > defaultDuration + ui.BackgroundShowClip.length)
        {
            return clipDuration + minimumTimeBetweenLines;
        }
        else
        {
            return defaultDuration + ui.BackgroundShowClip.length + minimumTimeBetweenLines;
        }
    }
}