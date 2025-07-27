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
    StringListMax inputHistory = new(20);

    HashSet<string> prepositions = new HashSet<string>
        {
            "a", "an", "the", "go", "into", "in",
        };

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
        // Updates the main story display with text
        StoryTextHandler.invokeUpdateStoryDisplay("> " + TmpTextTagger.Color(inputText, UiConstants.TEXT_COLOR_PLAYER_ACTION));

        inputText = NormalizeInput(inputText);

        string[] inputTextArray = inputText.Split(' ');

        inputTextArray = ActionPhraseSynonyms.getInstance().Parse(inputTextArray);

        inputTextArray = inputTextArray.Where(word => !prepositions.Contains(word)).ToArray();

        string action = ActionWordSynonyms.Get(inputTextArray[0]);

        if (action != null)
        {
            // LoggingUtil.Log("Player Action was : " + action + " | inputTextArray: " + actionInput.mainClause.Count);
            // LoggingUtil.LogList(inputTextArray.ToList());

            IPlayerAction playerAction = ActionRegistry.Get(action);

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
                playerAction.Execute(new ActionInput
                {
                    actionTaken = action,
                    mainClause = inputTextArray.Skip(1).ToList()
                });
            }
        }
        else
        {
            Log.Debug("Action was null, no corresponding action for: " + inputTextArray[0]);
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
        string normalized = input.ToLower().Trim();
        normalized = Regex.Replace(normalized, @"\b('s)\b", "").Replace("  ", " ").Trim();
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
