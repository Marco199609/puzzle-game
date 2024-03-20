using SnowHorse.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdySceneController : MonoBehaviour
{
    [SerializeField] private Animator butterflyAnimator;
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private Animator catGlowAnimator;
    [SerializeField] private AnimationClip butterflyClip;
    [SerializeField] private Image plantImage;
    [SerializeField] private Image catImage;
    [SerializeField] private Color catTargetColor;
    void Start()
    {
        plantImage.fillAmount = 0;

        StartCoroutine(CatColorControl());
    }

    private IEnumerator CatColorControl()
    {
        yield return new WaitForSeconds(2f);
        butterflyAnimator.SetBool("start", true);
        yield return new WaitForSecondsRealtime(butterflyClip.length + 0.8f);
        catGlowAnimator.SetBool("start", true);
        yield return new WaitForSecondsRealtime(0.4f);

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

        plantImage.fillClockwise = true;

        while (percentage < 1)
        {
            percentage = Interpolation.Linear(1f, ref lerpRef);
            plantImage.fillAmount = Mathf.Lerp(0, 1, percentage);

            yield return null;
        }

        percentage = 0;
        lerpRef = 0;

        plantImage.fillClockwise = false;

        while(percentage < 1)
        {
            percentage = Interpolation.Linear(1f, ref lerpRef);
            plantImage.fillAmount = Mathf.Lerp(1, 0, percentage);

            yield return null;
        }
    }
}
