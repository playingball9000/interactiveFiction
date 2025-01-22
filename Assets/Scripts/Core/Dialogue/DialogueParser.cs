using System.Collections.Generic;
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
    private List<DialogueChoice> currentChoices = new();
    private List<Fact> dialogueFacts = new();

    StringListMax dialogueLog = new(100);

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
            for (int i = 0; i < currentChoices.Count; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKeyDown(numpadKeys[i + 1]))
                {
                    DialogueChoice choice = currentChoices[i];
                    OnChoiceSelected(choice.text, choice.nextNode);
                }
            }
        }
    }

    public void StartDialogue(NPC npc)
    {
        dialogueLog.Clear();
        currentChoices.Clear();
        dialogueFacts.Clear();
        GameController.invokeShowDialogueCanvas();

        //TODO: Finish gathering facts here, npc, location, world, context, etc
        dialogueFacts.AddRange(WorldState.GetInstance().player.currentLocation.GetRoomFacts());
        dialogueFacts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        DialogueNode startNode = DialogueBuilder.BuildDialogue(npc.dialogueFile);
        DisplayDialogue(startNode);
    }

    public void DisplayDialogue(DialogueNode dialogueNode)
    {
        currentNode = dialogueNode;
        if (currentNode != null)
        {
            string fullText =
                TmpTextTagger.Color($"{currentNode.speaker}   -   ", UiConstants.TEXT_COLOR_NPC_NAME) +
                TmpTextTagger.Color($"{currentNode.text}", UiConstants.TEXT_COLOR_NPC_TEXT);
            dialogueLog.Add(fullText + "\n");
            UI_dialogueBox.text = dialogueLog.GetLogsString();
            ScrollToBottom();

            UI_dialogueChoicesBox.text = "";

            int choiceCounter = 1;
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                // If no rule or if rule passes, add choice
                if (currentNode.choices[i].showRule == null || currentNode.choices[i].showRule.Evaluate(dialogueFacts))
                {
                    // Not all choices satisfy rules so need to keep track of choices that passed
                    currentChoices.Add(currentNode.choices[i]);
                    UI_dialogueChoicesBox.text += $"{choiceCounter}. {currentNode.choices[i].text}\n";
                    choiceCounter++;
                }
            }
        }
    }

    public void OnChoiceSelected(string choiceText, DialogueNode nextNode)
    {
        currentChoices.Clear();

        if (nextNode == null)
        {
            dialogueLog.Clear();
            GameController.invokeShowMainCanvas();
        }
        else
        {
            string coloredText =
                TmpTextTagger.Color($"You   -   ", UiConstants.TEXT_COLOR_PLAYER_NAME) +
                TmpTextTagger.Color($"{choiceText}", UiConstants.TEXT_COLOR_PLAYER_TEXT);
            dialogueLog.Add(coloredText + "\n");
            DisplayDialogue(nextNode);
        }
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        dialogueScrollRect.verticalNormalizedPosition = 0f;
    }
}

