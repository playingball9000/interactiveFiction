using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI UI_dialogueBox;
    [HideInInspector]
    public TextMeshProUGUI UI_dialogueChoicesBox;
    [HideInInspector]
    public GameObject UI_storyScrollView;
    private ScrollRect dialogueScrollRect;

    private DialogueNode currentNode;
    private Dictionary<string, DialogueNode> dialogues;

    StringListMax dialogueLog = new StringListMax(100);

    KeyCode[] numpadKeys = { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
                         KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };


    public delegate void StartDialogueDelegate(NPC npc);
    public static StartDialogueDelegate invokeStartDialogue;

    void Awake()
    {
        GameObject dialogueTextObject = GameObject.Find("DialogueTextBox");
        UI_dialogueBox = dialogueTextObject.GetComponent<TextMeshProUGUI>();

        GameObject choicesTextObject = GameObject.Find("ChoicesTextBox");
        UI_dialogueChoicesBox = choicesTextObject.GetComponent<TextMeshProUGUI>();

        UI_storyScrollView = GameObject.Find("DialogueScrollView");
        dialogueScrollRect = UI_storyScrollView.GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        DialogueParser.invokeStartDialogue += StartDialogue;
    }

    void OnDisable()
    {
        DialogueParser.invokeStartDialogue -= StartDialogue;
    }

    void Update()
    {
        if (WorldState.GetInstance().FLAG_dialogWindowActive && currentNode != null && Input.anyKeyDown)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKeyDown(numpadKeys[i + 1]))
                {
                    DialogueChoice choice = currentNode.choices[i];
                    OnChoiceSelected(choice.text, choice.nextId);
                }
            }
        }
    }

    public void StartDialogue(NPC npc)
    {
        dialogueLog.Clear();
        GameController.invokeShowDialogueCanvas();

        // This takes about 45 ms to run for a 100 line file. Not sure if performance degrades
        var jsonText = Resources.Load<TextAsset>("Dialogue/" + npc.dialogueFile).text;
        dialogues = JsonConvert.DeserializeObject<Dictionary<string, DialogueNode>>(jsonText);

        DisplayDialogue("node1");
    }

    public void DisplayDialogue(string dialogueId)
    {
        currentNode = dialogues.ContainsKey(dialogueId) ? dialogues[dialogueId] : null;
        if (currentNode != null)
        {
            string fullText = TmpTextTagger.Color($"{currentNode.speaker}   -   ", UiConstants.TEXT_COLOR_NPC_NAME) + TmpTextTagger.Color($"{currentNode.text}", UiConstants.TEXT_COLOR_NPC_TEXT);
            dialogueLog.Add(fullText + "\n");
            UI_dialogueBox.text = dialogueLog.GetLogsString();
            ScrollToBottom();

            UI_dialogueChoicesBox.text = "";
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                UI_dialogueChoicesBox.text += $"{i + 1}. {currentNode.choices[i].text}\n";
            }
        }
        else
        {
            LoggingUtil.Log($"Dialogue with ID '{dialogueId}' not found.");
        }
    }

    public void OnChoiceSelected(string choiceText, string nextDialogueId)
    {
        if (nextDialogueId == "END_DIALOGUE")
        {
            dialogueLog.Clear();
            GameController.invokeShowMainCanvas();
        }
        else
        {
            string coloredText = TmpTextTagger.Color($"You   -   ", UiConstants.TEXT_COLOR_PLAYER_NAME) + TmpTextTagger.Color($"{choiceText}", UiConstants.TEXT_COLOR_PLAYER_TEXT);
            dialogueLog.Add(coloredText + "\n");
            DisplayDialogue(nextDialogueId);
        }
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        dialogueScrollRect.verticalNormalizedPosition = 0f;
    }
}

