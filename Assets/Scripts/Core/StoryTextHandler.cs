using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryTextHandler : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI UI_storyBox;
    [HideInInspector]
    public GameObject UI_storyScrollView;
    private ScrollRect storyScrollRect;

    StringListMax storyLog = new(250);

    private Coroutine typingCoroutine;
    WaitForSeconds textQueueWait = new(0.7f);
    WaitForSeconds typeWritingWait = new(0.015f);
    private Queue<(string text, TextEffect effect)> textQueue = new();
    private bool isTyping = false;
    private bool skipQueueWait = false;

    // Delegates for other functions to use to invoke this. Probably not the right way but oh well.
    public delegate void UpdateStoryDisplayDelegate(string text, TextEffect effect = TextEffect.None);
    public static UpdateStoryDisplayDelegate invokeUpdateStoryDisplay;

    void Awake()
    {
        // Has to match game object name
        GameObject storyBoxObject = GameObject.Find("StoryTextBox");
        UI_storyBox = storyBoxObject.GetComponent<TextMeshProUGUI>();
        UI_storyBox.text = "";

        UI_storyScrollView = GameObject.Find("StoryScrollView");
        storyScrollRect = UI_storyScrollView.GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        invokeUpdateStoryDisplay += UpdateStoryDisplay;

    }

    void OnDisable()
    {
        invokeUpdateStoryDisplay -= UpdateStoryDisplay;
    }

    public void UpdateStoryDisplay(string text, TextEffect effect)
    {
        text = TmpTextTagger.Color(text, UiConstants.TEXT_COLOR_STORY_TEXT);
        storyLog.Add(text + "\n");

        // Using a queue here in case multiple updates come through at once so each one is handled in order
        textQueue.Enqueue((text, effect));
        if (!isTyping)
        {
            typingCoroutine = StartCoroutine(ProcessTextQueue());
        }

        UiUtilMb.Instance.ScrollToBottom(storyScrollRect);
    }

    private IEnumerator ProcessTextQueue()
    {
        isTyping = true;
        while (textQueue.Count > 0)
        {
            skipQueueWait = false;
            var (nextText, effect) = textQueue.Dequeue();

            if (TextEffect.None == effect)
            {
                UI_storyBox.text = storyLog.GetLogsString();
            }
            else if (TextEffect.Typewriter == effect)
            {
                yield return StartCoroutine(TypewriterAppend(nextText));
                if (!skipQueueWait)
                {
                    yield return textQueueWait;
                }
            }
        }
        isTyping = false;
    }

    private IEnumerator TypewriterAppend(string newText)
    {
        int charIndex = 0;

        while (charIndex < newText.Length)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Stop coroutine and just display the text
                UI_storyBox.text += newText;
                skipQueueWait = true;
                yield break;
            }
            // Skips typewriting the tags
            if (newText[charIndex] == '<')
            {
                int tagEnd = newText.IndexOf('>', charIndex) - 1;
                if (tagEnd != -1)
                {
                    UI_storyBox.text += newText.Substring(charIndex, tagEnd - charIndex + 1);
                    charIndex = tagEnd + 1;
                    continue;
                }
            }
            UI_storyBox.text += newText[charIndex];
            charIndex++;
            yield return typeWritingWait;
        }
    }
}
