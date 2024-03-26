using SnowHorse.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject body;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;


    private float preferredWidth;
    private Vector2 startSize;
    private RectTransform bodyRectTransform;

    private void Start()
    {
        StartCoroutine(SetBodyWidth());
    }

    private IEnumerator SetBodyWidth()
    {
        text.color = new Color();
        bodyRectTransform = body.GetComponent<RectTransform>();
        preferredWidth = text.preferredWidth;
        startSize = bodyRectTransform.sizeDelta;

        var percent = 0f;
        var lerpRef = 0f;

        var targetSize = new Vector2(preferredWidth, bodyRectTransform.sizeDelta.y);

        while (percent < 1)
        {
            percent = Interpolation.Smoother(1.5f, ref lerpRef);

            bodyRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, percent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bodyRectTransform);
            yield return null;
        }
        text.color = Color.black;

        yield return StartCoroutine(TextBubbleDuration(3));
    }

    private IEnumerator TextBubbleDuration(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        text.color = new Color();

        var percent = 0f;
        var lerpRef = 0f;

        var startSize = bodyRectTransform.sizeDelta;
        var targetSize = this.startSize;

        while (percent < 1)
        {
            percent = Interpolation.Smoother(1.5f, ref lerpRef);

            bodyRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, percent);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bodyRectTransform);

            if(percent >= 0.7f)
            {
                animator.SetBool("fade", true);
            }


            yield return null;
        }

        Destroy(parent);
    }
}
