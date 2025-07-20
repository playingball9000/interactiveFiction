using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TaskMessageUI : MonoBehaviour
{
    public TMP_Text messageText;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    private Coroutine messageRoutine;

    public void ShowMessage(string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(DisplayMessageRoutine(message));
    }

    private IEnumerator DisplayMessageRoutine(string message)
    {
        messageText.text = message;

        // Fade in
        yield return FadeCanvas(0f, 1f, fadeDuration);

        // Wait
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return FadeCanvas(1f, 0f, fadeDuration);
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
