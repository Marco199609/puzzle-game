using SnowHorse.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdySceneController : MonoBehaviour
{
    [SerializeField] private Animator butterflyAnimator;
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private AnimationClip butterflyClip;
    [SerializeField] private Image plantImage;
    [SerializeField] private Image catImage;
    [SerializeField] private Color catTargetColor;
    void Start()
    {
        butterflyAnimator.SetBool("start", true);
        StartCoroutine(CatColorControl());
    }

    private IEnumerator CatColorControl()
    {
        yield return new WaitForSecondsRealtime(butterflyClip.length + 1f);

        var percentage = 0f;
        var lerpRef = 0f;

        while (percentage < 1)
        {
            percentage = Interpolation.Linear(0.5f, ref lerpRef);
            catImage.color = Color.Lerp(Color.black, catTargetColor, percentage);

            yield return null;
        }

        yield return StartCoroutine(PlantFillControl());
    }

    private IEnumerator PlantFillControl()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        heartAnimator.SetBool("start", true);

        var percentage = 0f;
        var lerpRef = 0f;

        while(percentage < 1)
        {
            percentage = Interpolation.Linear(1.5f, ref lerpRef);
            plantImage.fillAmount = Mathf.Lerp(0, 1, percentage);

            yield return null;
        }
    }
}
