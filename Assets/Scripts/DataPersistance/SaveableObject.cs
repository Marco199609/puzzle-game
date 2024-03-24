using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SaveableObject : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }

    private float delayInSeconds = 30;

    private void OnEnable()
    {
        EditorApplication.update += GenerateGUID;
    }

    private void GenerateGUID()
    {
        if(EditorApplication.timeSinceStartup < delayInSeconds && (string.IsNullOrEmpty(Id)))
        {
            int timeLeft = (int)delayInSeconds - (int)EditorApplication.timeSinceStartup;
            Debug.Log($"Will attempt {gameObject.name} GUID update in: {timeLeft}.");
        }
        else
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                Debug.Log($"Generated new GUID for {gameObject.name}. If this object existed previously, save file deletion is recommended.");
            }
            else
            {
                Debug.Log($"Detected existing GUID for {gameObject.name}.");
            }

            EditorApplication.update -= GenerateGUID;
        }
    }

    private void OnDestroy()
    {
        EditorApplication.update -= GenerateGUID;
    }
}