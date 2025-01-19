using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryTextHandler : MonoBehaviour
{
    public TextMeshProUGUI UI_storyBox;
    public GameObject storyScrollView;
    private ScrollRect storyScrollRect;

    List<string> storyLog = new List<string>();

    // Delegates for other functions to use to invoke this. Probably not the right way but oh well.
    public delegate void UpdateStoryDisplayDelegate(string text);
    public static UpdateStoryDisplayDelegate invokeUpdateStoryDisplay;

    void Start()
    {
        storyScrollRect = storyScrollView.GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        StoryTextHandler.invokeUpdateStoryDisplay += UpdateStoryDisplay;
    }

    private void OnDisable()
    {
        StoryTextHandler.invokeUpdateStoryDisplay -= UpdateStoryDisplay;
    }

    public void UpdateStoryDisplay(string text)
    {
        text = TmpTextTagger.Color(text, UiConstants.TEXT_COLOR_STORY_TEXT);
        storyLog.Add(text + "\n");
        string logAsText = string.Join("\n", storyLog.ToArray());

        UI_storyBox.text = logAsText;
        ScrollToBottom();

    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        storyScrollRect.verticalNormalizedPosition = 0f;
    }
}
