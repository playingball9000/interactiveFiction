using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUtilMb : MonoBehaviour
{
    WaitForSeconds typeWritingWait = new(0.015f);
    WaitForSeconds textQueueWait = new(0.7f);

    private static UiUtilMb _instance;

    public static UiUtilMb Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject util = new GameObject("UiUtilMb");
                _instance = util.AddComponent<UiUtilMb>();
                DontDestroyOnLoad(util);
            }
            return _instance;
        }
    }

    public void DestroyChildrenInContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    public void ScrollToBottom(ScrollRect scrollRect)
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public IEnumerator TypewriterAppend(string newText, TextMeshProUGUI tmpBox)
    {
        int charIndex = 0;
        newText = TMPTagCleaner.Clean(newText);

        while (charIndex < newText.Length)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Stop coroutine and just display the text
                tmpBox.text += newText;
                // skipQueueWait = true;
                yield break;
            }
            // Skips typewriting the tags
            if (newText[charIndex] == '<')
            {
                int tagEnd = newText.IndexOf('>', charIndex) - 1;
                if (tagEnd != -1)
                {
                    tmpBox.text += newText.Substring(charIndex, tagEnd - charIndex + 1);
                    charIndex = tagEnd + 1;
                    continue;
                }
            }
            tmpBox.text += newText[charIndex];
            charIndex++;
            yield return typeWritingWait;
        }
    }
}