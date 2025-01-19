using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [HideInInspector]
    public TMP_InputField UI_playerInputBox;

    private int currentHistoryIndex = -1;
    StringListMax inputHistory = new StringListMax(20);

    //TODO: for later complex inputs
    private static readonly HashSet<string> prepositions = new HashSet<string> { "in", "on", "under", "into", "onto" };

    void Awake()
    {
        // Has to match game object name
        GameObject inputFieldObject = GameObject.Find("PlayerInputBox");
        UI_playerInputBox = inputFieldObject.GetComponent<TMP_InputField>();
    }

    void OnEnable()
    {
        GameController.invokeShowMainCanvas += ActivateAndClearField;
    }

    void OnDisable()
    {
        GameController.invokeShowMainCanvas -= ActivateAndClearField;
    }

    void Start()
    {
        // Enter or Return while box is focused will call the GrabTextFromInputField() function
        UI_playerInputBox.onSubmit.AddListener(GrabTextFromInputField);
    }

    void Update()
    {
        // Handles navigating the history of inputs
        if (UI_playerInputBox.isFocused && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateInputHistory(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateInputHistory(1);
            }
        }
    }

    void OnDestroy()
    {
        UI_playerInputBox.onSubmit.RemoveListener(GrabTextFromInputField);
    }

    public void GrabTextFromInputField(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            return;
        }

        AddToInputHistory(inputText);
        inputText = NormalizeInput(inputText);

        // Updates the main story display with text
        StoryTextHandler.invokeUpdateStoryDisplay("> " + TmpTextTagger.Color(inputText, UiConstants.TEXT_COLOR_PLAYER_ACTION));

        string[] inputTextArray = inputText.Split(' ');

        // remove go if it is the first input. ie. go north or go examine
        if (inputTextArray[0].ToLower() == "go")
        {
            inputTextArray = inputTextArray.Skip(1).ToArray();
        }

        string action = ActionSynonyms.SynonymsDict.ContainsKey(inputTextArray[0]) ? ActionSynonyms.SynonymsDict[inputTextArray[0]] : null;

        // Putting this here for movement abbreviations so n is replaced by north
        inputTextArray[0] = action;

        if (action != null)
        {
            // LoggingUtil.Log("Player Action was : " + action + " | inputTextArray: " + inputTextArray.Length);
            // LoggingUtil.LogList(inputTextArray.ToList());

            //TODO: I should really parse the inputTextArray here and then send the relevant values (direct object, target, item, etc) to the action.
            IPlayerAction playerAction = ActionRegistry.ActionsDict[action];

            if (inputTextArray.Length < playerAction.minInputCount)
            {
                StoryTextHandler.invokeUpdateStoryDisplay(playerAction.tooFewMessage);
            }
            else if (inputTextArray.Length > playerAction.maxInputCount)
            {
                StoryTextHandler.invokeUpdateStoryDisplay(playerAction.tooManyMessage);
            }
            else
            {
                playerAction.Execute(inputTextArray);
            }
        }
        else
        {
            LoggingUtil.Log("Action was null, no corresponding action for: " + inputTextArray[0]);
            StoryTextHandler.invokeUpdateStoryDisplay(ActionUtil.GetUnknownCommandResponse());
        }
        ActivateAndClearField();
    }

    public void ActivateAndClearField()
    {
        UI_playerInputBox.ActivateInputField();
        UI_playerInputBox.text = "";
    }

    private string NormalizeInput(string input)
    {
        var normalized = input.ToLower().Trim();
        normalized = Regex.Replace(normalized, @"\b(a|an|the)\b", "").Trim();
        normalized = normalized.Replace("'s ", "").Replace("  ", " ");
        return normalized;
    }

    private void NavigateInputHistory(int direction)
    {
        if (inputHistory.Count == 0 || (currentHistoryIndex == -1 && direction == 1)) return;
        if (direction == 1 && currentHistoryIndex == inputHistory.Count - 1)
        {
            currentHistoryIndex = -1;
            UI_playerInputBox.text = "";
            return;
        }
        if (currentHistoryIndex == -1)
        {
            currentHistoryIndex = inputHistory.Count;
        }

        currentHistoryIndex = Mathf.Clamp(currentHistoryIndex + direction, 0, inputHistory.Count - 1);

        UI_playerInputBox.text = inputHistory.Get(currentHistoryIndex);
        UI_playerInputBox.caretPosition = UI_playerInputBox.text.Length;
    }

    private void AddToInputHistory(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            currentHistoryIndex = -1;
            inputHistory.Add(input);
        }
    }

}
