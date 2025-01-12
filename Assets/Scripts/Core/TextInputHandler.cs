using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class GrabTextFromInput : MonoBehaviour
{
    public TMP_InputField inputField;


    private List<string> inputHistory = new List<string>();
    private int currentHistoryIndex = -1;
    private int MAX_HISTORY_LENGTH = 20;

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
        if (inputField == null)
        {
            GameObject inputFieldObject = GameObject.Find("PlayerActionInputField");
            inputField = inputFieldObject.GetComponent<TMP_InputField>();
        }

        inputField.onSubmit.AddListener(GrabTextFromInputField);
    }

    void Update()
    {
        if (inputField.isFocused)
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
        inputField.onSubmit.RemoveListener(GrabTextFromInputField);
    }

    public void GrabTextFromInputField(string inputText)
    {
        AddToInputHistory(inputText);
        inputText = NormalizeInput(inputText);

        DisplayTextHandler.invokeUpdateTextDisplay("> " + inputText);

        string[] inputTextArray = inputText.Split(' ');

        // remove go if it is the first input. ie. go north or go examine
        if (inputTextArray[0].ToLower() == "go")
        {
            inputTextArray = inputTextArray.Skip(1).ToArray();
        }

        string action = ActionSynonyms.SynonymsDict.ContainsKey(inputTextArray[0]) ? ActionSynonyms.SynonymsDict[inputTextArray[0]] : null;

        if (action != null)
        {
            inputTextArray[0] = action;
            GameController.invokeProcessPlayerAction(inputTextArray);

        }
        else
        {
            LoggingUtil.Log("Action was null, no corresponding action for: " + inputTextArray[0]);
            DisplayTextHandler.invokeUpdateTextDisplay("Unknown command");
        }
        ActivateInputTextField();
        inputField.text = "";
    }

    public void ActivateInputTextField()
    {
        inputField.ActivateInputField();

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
            inputField.text = "";
            return;
        }
        if (currentHistoryIndex == -1)
        {
            currentHistoryIndex = inputHistory.Count;
        }

        currentHistoryIndex = Mathf.Clamp(currentHistoryIndex + direction, 0, inputHistory.Count - 1);

        inputField.text = inputHistory[currentHistoryIndex];
        inputField.caretPosition = inputField.text.Length;
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
