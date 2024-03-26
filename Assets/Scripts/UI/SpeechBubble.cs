using SnowHorse.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject notch;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private float preferredWidth;
    private float fadeThreshold = 0.7f;
    private float lerpDuration = 0.8f;
    private Vector2 startSize;
    private RectTransform bodyRectTransform;

    public void Set(string message, float duration, DialogueType type)
    {
        text.text = message;
        preferredWidth = text.preferredWidth;
        if(type == DialogueType.Thought) notch.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(SetBodyWidth(duration));
    }

    private IEnumerator SetBodyWidth(float duration)
    {
        text.color = new Color();
        bodyRectTransform = body.GetComponent<RectTransform>();
        
        var percent = 0f;
        var lerpRef = 0f;
        startSize = bodyRectTransform.sizeDelta;
        var targetSize = new Vector2(preferredWidth, bodyRectTransform.sizeDelta.y);

        while (percent < 1)
        {
            percent = Interpolation.Smoother(lerpDuration, ref lerpRef);
            bodyRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, percent);
            yield return null;
        }

        text.color = Color.black;
        yield return StartCoroutine(TextBubbleDuration(duration));
    }

    private IEnumerator TextBubbleDuration(float duration)
    {
        yield return new WaitForSecondsRealtime(duration - lerpDuration);
        text.color = new Color();

        var percent = 0f;
        var lerpRef = 0f;
        var startSize = bodyRectTransform.sizeDelta;
        var targetSize = this.startSize;

        while (percent < 1)
        {
            percent = Interpolation.Smoother(lerpDuration, ref lerpRef);
            bodyRectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, percent);
            if(percent >= fadeThreshold) animator.SetBool("fade", true);
            yield return null;
        }

        Destroy(parent);
    }
}