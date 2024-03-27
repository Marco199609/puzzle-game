using System.Collections;
using UnityEngine;

public class CinematicsController : MonoBehaviour
{
    public bool CutsceneActive { get; private set; }

    public static CinematicsController Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }

    public void StartCutscene(float duration)
    {
        StartCoroutine(SetCutsceneState(duration));
    }

    private IEnumerator SetCutsceneState(float duration)
    {
        CutsceneActive = true;
        var totalDuration = duration + UIController.Instance.UiBarsClipDuration;
        Inventory.Instance.DeactivateInventory(totalDuration);
        yield return new WaitUntil(() => Inventory.Instance.IsInventoryInactive);

        UIController.Instance.ShowCutscene(duration);
        yield return new WaitForSecondsRealtime(totalDuration);

        CutsceneActive = false;
    }
}