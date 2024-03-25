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

    public DialogueData data;
    public static DialogueController Instance;

    private float timeBetweenDialogueLines = 1;
    private DialogueUI ui;
    private Coroutine manageDialogueCache;
    private List<(string, float)> dialogueCache = new List<(string, float)>();

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        data = JsonConvert.DeserializeObject<DialogueData>(json.text);
        ui = GetComponent<DialogueUI>();
    }

    public void PlayDialogue(int id, float duration)
    {
        string text;
        switch (language)
        {
            case Language.english:
                data.English.TryGetValue(id, out text);
                break;
            case Language.spanish:
                data.Spanish.TryGetValue(id, out text);
                break;
            default:
                data.English.TryGetValue(id, out text);
                break;
        }

        dialogueCache.Add((text, duration));
        if (manageDialogueCache == null) manageDialogueCache = StartCoroutine(ManageDialogueCache());

        Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
    }

    private IEnumerator ManageDialogueCache()
    {
        while (dialogueCache.Count > 0)
        {
            ui.ShowDialogueText(dialogueCache[0].Item1, dialogueCache[0].Item2);
            yield return new WaitForSecondsRealtime(dialogueCache[0].Item2 + ui.BackgroundShowClip.length + timeBetweenDialogueLines);
            dialogueCache.RemoveAt(0);
            Debug.Log($"Current dialogue lines in cache: {dialogueCache.Count}");
        }

        manageDialogueCache = null;
    }
}