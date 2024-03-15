using SnowHorse.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private List<GameObject> behaviours;
    public void Interact(bool isInteracting, bool isInspecting)
    {
        if(behaviours != null && behaviours.Count > 0)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].GetComponent<IBehaviour>().Behaviour(isInteracting, isInspecting);
            }
        }
        else
        {
            WarningTool.Print("Warning: no behaviours on:", gameObject);
        }
    }
}