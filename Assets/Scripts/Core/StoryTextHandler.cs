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

    StringListMax storyLog = new StringListMax(250);

    // Delegates for other functions to use to invoke this. Probably not the right way but oh well.
    public delegate void UpdateStoryDisplayDelegate(string text);
    public static UpdateStoryDisplayDelegate invokeUpdateStoryDisplay;

    void Awake()
    {
        // Has to match game object name
        GameObject storyBoxObject = GameObject.Find("StoryTextBox");
        UI_storyBox = storyBoxObject.GetComponent<TextMeshProUGUI>();

        UI_storyScrollView = GameObject.Find("StoryScrollView");
        storyScrollRect = UI_storyScrollView.GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        StoryTextHandler.invokeUpdateStoryDisplay += UpdateStoryDisplay;
    }

    void OnDisable()
    {
        StoryTextHandler.invokeUpdateStoryDisplay -= UpdateStoryDisplay;
    }

    public void UpdateStoryDisplay(string text)
    {
        text = TmpTextTagger.Color(text, UiConstants.TEXT_COLOR_STORY_TEXT);
        storyLog.Add(text + "\n");
        UI_storyBox.text = storyLog.GetLogsString();
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        storyScrollRect.verticalNormalizedPosition = 0f;
    }
}
