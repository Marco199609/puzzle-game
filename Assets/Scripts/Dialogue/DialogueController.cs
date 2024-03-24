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

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        data = JsonConvert.DeserializeObject<DialogueData>(json.text);
    }

    public void PlayDialogue(int id)
    {
        string text = string.Empty;
        switch (language)
        {
            case Language.english:
                data.English.TryGetValue(id, out text);
                break;
            case Language.spanish:
                data.Spanish.TryGetValue(id, out text);
                break;
        }

        //UI show text
    }


    void Update()
    {
        string text = string.Empty;
        switch (language)
        {
            case Language.english:
                data.English.TryGetValue(0, out text);
                break;
            case Language.spanish:
                data.Spanish.TryGetValue(0, out text);
                break;
        }

        print(text);
    }
}