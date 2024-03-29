using SnowHorse.Utils;
using System.Collections.Generic;
using UnityEngine;
public interface IBehaviour
{
    public void Behaviour(bool isInteracting, bool isInspecting);
}

public class Interactable : MonoBehaviour
{
    [SerializeField] private List<GameObject> behaviours;
    public void Interact(bool isInteracting, bool isInspecting)
    {
        if(behaviours != null && behaviours.Count > 0)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                var behaviourComponents = behaviours[i].GetComponents<IBehaviour>();

                for(int j = 0; j < behaviourComponents.Length; j++)
                {
                    behaviourComponents[j].Behaviour(isInteracting, isInspecting);
                }
            }
        }
        else
        {
            WarningTool.Print("Warning: no behaviours on:", gameObject);
        }
    }
}