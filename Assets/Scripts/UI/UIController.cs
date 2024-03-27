using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;

    [Header("Cursor")]
    [SerializeField] private Texture2D cross;
    [SerializeField] private Texture2D circle;
    [SerializeField] private Animator cinematicBarsAnimator;
    [Header("Cutscene")]
    [SerializeField] private AnimationClip hideBarsClip;

    public float UiBarsClipDuration { get => hideBarsClip.length + 1.5f; }

    public static UIController Instance;

    private void Awake()
    {
        if(Instance) Destroy(this);
        else Instance = this;
    }

    #region Cursor
    public void SetCursor(bool canInteract)
    {
        var cursor = canInteract ? circle : cross;
        Cursor.SetCursor(cursor, new Vector2(cursor.height / 2, cursor.width / 2), CursorMode.Auto);
    }
    #endregion

    #region Cinematic bars

    public void ShowCutscene(float duration)
    {
        StartCoroutine(CinematicBarControl(duration));
    }

    private IEnumerator CinematicBarControl(float duration)
    {
        cinematicBarsAnimator.SetBool("activate", true);
        yield return new WaitForSecondsRealtime(duration);
        cinematicBarsAnimator.SetBool("activate", false);
    }
    #endregion
}