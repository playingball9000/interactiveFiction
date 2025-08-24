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

    private Queue<(string text, TextEffect effect)> textQueue = new();
    private bool isTyping = false;

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
            StartCoroutine(ProcessTextQueue(UI_storyBox));
        }

        UiUtilMb.Instance.ScrollToBottom(storyScrollRect);
    }

    private IEnumerator ProcessTextQueue(TextMeshProUGUI tmpBox)
    {
        isTyping = true;
        while (textQueue.Count > 0)
        {
            var (nextText, effect) = textQueue.Dequeue();

            if (TextEffect.None == effect)
            {
                tmpBox.text = storyLog.GetLogsString();
            }
            else if (TextEffect.Typewriter == effect)
            {
                yield return StartCoroutine(UiUtilMb.Instance.TypewriterAppend(nextText, tmpBox));
            }
        }
        isTyping = false;
    }
}
