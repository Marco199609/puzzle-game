using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_ActivateCutscene : MonoBehaviour, IBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float duration;
    public void Behaviour(bool isInteracting, bool isInspecting)
    {
        UIController.Instance.StartCoroutine(ActivateCutscene()); 
    }

    private IEnumerator ActivateCutscene()
    {
        yield return new WaitForSecondsRealtime(delay);
        UIController.Instance.ShowCutscene(duration);
    }
}
