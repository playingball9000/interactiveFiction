using System.Collections;
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

    private DialogueGraph dialogueGraph;
    private List<DialogueChoice> currentChoices = new();
    private List<Fact> dialogueFacts = new();

    StringListMax dialogueLog = new(120);
    WaitForSeconds dialogueWait = new WaitForSeconds(0.25f);

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
        if (WorldState.GetInstance().FLAG_dialogWindowActive && Input.anyKeyDown)
        {
            for (int i = 0; i < currentChoices.Count; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKeyDown(numpadKeys[i + 1]))
                {
                    DialogueChoice choice = currentChoices[i];
                    OnChoiceSelected(choice);
                }
            }
        }
    }

    public void StartDialogue(NPC npc)
    {
        dialogueLog.Clear();
        currentChoices.Clear();
        dialogueFacts.Clear();
        UI_dialogueChoicesBox.text = "";
        GameController.invokeShowDialogueCanvas();

        //TODO: Finish gathering facts here, npc, location, world, context, etc
        dialogueFacts.AddRange(WorldState.GetInstance().player.currentLocation.GetRoomFacts());
        dialogueFacts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        dialogueGraph = DialogueBuilder.BuildDialogue(npc.dialogueFile);
        DisplayDialogue();
    }

    public void DisplayDialogue()
    {
        DialogueNode currentNode = dialogueGraph.currentNode;
        string fullText =
            TmpTextTagger.Color($"{currentNode.speaker}   -   ", UiConstants.TEXT_COLOR_NPC_NAME) +
            TmpTextTagger.Color($"{currentNode.text}", UiConstants.TEXT_COLOR_NPC_TEXT);
        dialogueLog.Add(fullText + "\n");
        UI_dialogueBox.text = dialogueLog.GetLogsString();
        ScrollToBottom();


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

    public void OnChoiceSelected(DialogueChoice choice)
    {
        currentChoices.Clear();
        UI_dialogueChoicesBox.text = "";

        if (choice.nextNode == null)
        {
            dialogueLog.Clear();
            GameController.invokeShowMainCanvas();
        }
        else
        {
            if (choice.executeCode != null)
            {
                choice.Execute();
            }
            dialogueGraph.SetCurrentNode(choice.nextNode);
            StartCoroutine(DisplayDialogueWithDelay(choice.text));
        }
    }

    IEnumerator DisplayDialogueWithDelay(string choiceText)
    {
        string coloredText =
                TmpTextTagger.Color($"You   -   ", UiConstants.TEXT_COLOR_PLAYER_NAME) +
                TmpTextTagger.Color($"{choiceText}", UiConstants.TEXT_COLOR_PLAYER_TEXT);
        dialogueLog.Add(coloredText + "\n");
        UI_dialogueBox.text = dialogueLog.GetLogsString();

        yield return dialogueWait;

        DisplayDialogue();

    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        dialogueScrollRect.verticalNormalizedPosition = 0f;
    }
}

