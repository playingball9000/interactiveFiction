using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public TMP_InputField UI_playerInputBox;


    private List<string> inputHistory = new List<string>();
    private int currentHistoryIndex = -1;
    private int MAX_HISTORY_LENGTH = 20;

    //TODO: for later complex inputs
    private static readonly HashSet<string> prepositions = new HashSet<string> { "in", "on", "under", "into", "onto" };

    private void OnEnable()
    {
        GameController.invokeShowMainCanvas += ActivateInputTextField;
    }

    private void OnDisable()
    {
        GameController.invokeShowMainCanvas -= ActivateInputTextField;
    }

    void Start()
    {
        if (UI_playerInputBox == null)
        {
            GameObject inputFieldObject = GameObject.Find("PlayerActionInputField");
            UI_playerInputBox = inputFieldObject.GetComponent<TMP_InputField>();
        }

        UI_playerInputBox.onSubmit.AddListener(GrabTextFromInputField);
    }

    void Update()
    {
        if (UI_playerInputBox.isFocused)
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
        AddToInputHistory(inputText);
        inputText = NormalizeInput(inputText);

        StoryTextHandler.invokeUpdateTextDisplay("> " + inputText);

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
            LoggingUtil.Log("Player Action was : " + action + " | inputTextArray: " + inputTextArray.Length);
            LoggingUtil.LogList(inputTextArray.ToList());
            IPlayerAction playerAction = ActionRegistry.ActionsDict[action];

            if (inputTextArray.Length < playerAction.minInputCount)
            {
                StoryTextHandler.invokeUpdateTextDisplay(playerAction.tooFewMessage);
            }
            else if (inputTextArray.Length > playerAction.maxInputCount)
            {
                StoryTextHandler.invokeUpdateTextDisplay(playerAction.tooManyMessage);
            }
            else
            {
                playerAction.Execute(inputTextArray);
            }
        }
        else
        {
            LoggingUtil.Log("Action was null, no corresponding action for: " + inputTextArray[0]);
            StoryTextHandler.invokeUpdateTextDisplay(ActionUtil.GetUnknownCommandResponse());
        }
        ActivateInputTextField();
        UI_playerInputBox.text = "";
    }

    public void ActivateInputTextField()
    {
        UI_playerInputBox.ActivateInputField();

    }

    private string NormalizeInput(string input)
    {
        var normalized = input.ToLower().Trim();
        normalized = Regex.Replace(normalized, @"\b(a|an|the)\b", "").Trim();
        normalized = normalized.Replace("'s ", " ").Replace("  ", " ");
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

        UI_playerInputBox.text = inputHistory[currentHistoryIndex];
        UI_playerInputBox.caretPosition = UI_playerInputBox.text.Length;
    }

    private void AddToInputHistory(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            currentHistoryIndex = -1;
            inputHistory.Add(input);
            if (inputHistory.Count > MAX_HISTORY_LENGTH)
            {
                inputHistory.RemoveAt(0);
            }
        }
    }

}
