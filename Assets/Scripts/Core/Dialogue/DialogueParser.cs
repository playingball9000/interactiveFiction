using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI UI_dialogueBox;

    public Transform choicesPanel;
    public GameObject choicePrefab;

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


    public delegate void StartDialogueDelegate(ComplexNPC npc);
    public static StartDialogueDelegate invokeStartDialogue;

    void Awake()
    {
        GameObject dialogueTextObject = GameObject.Find("DialogueTextBox");
        UI_dialogueBox = dialogueTextObject.GetComponent<TextMeshProUGUI>();

        choicesPanel = GameObject.Find("ChoicesPanel").transform;
        choicePrefab = (GameObject)Resources.Load("prefabs/dialogue/DialogueChoice", typeof(GameObject));

        UI_storyScrollView = GameObject.Find("DialogueScrollView");
        dialogueScrollRect = UI_storyScrollView.GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        invokeStartDialogue += StartDialogue;
        DialogueChoiceUI.OnChoiceClicked += HandleChoiceClicked;
    }

    void OnDisable()
    {
        invokeStartDialogue -= StartDialogue;
        DialogueChoiceUI.OnChoiceClicked -= HandleChoiceClicked;
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

    public void HandleChoiceClicked(int choiceNumber)
    {
        // Choice numbers start at 1
        DialogueChoice choice = currentChoices[choiceNumber - 1];
        OnChoiceSelected(choice);
    }

    public void StartDialogue(ComplexNPC npc)
    {
        dialogueLog.Clear();
        currentChoices.Clear();
        dialogueFacts.Clear();
        GameController.invokeShowDialogueCanvas();

        //TODO: Finish gathering facts here, npc, location, world, context, etc
        dialogueFacts.AddRange(PlayerContext.Get.currentRoom.GetRoomFacts());
        dialogueFacts.AddRange(PlayerContext.Get.GetPlayerFacts());

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
        DisplayPlayerChoices(currentNode);
    }

    private void DisplayPlayerChoices(DialogueNode currentNode)
    {
        // Display Player choices
        int choiceCounter = 1;
        for (int i = 0; i < currentNode.choices.Count; i++)
        {
            // If no rule or if rule passes, add choice
            if (currentNode.choices[i].showRule == null || currentNode.choices[i].showRule.Evaluate(dialogueFacts))
            {
                GameObject newChoice = Instantiate(choicePrefab, choicesPanel);
                DialogueChoiceUI choiceUI = newChoice.GetComponent<DialogueChoiceUI>();
                choiceUI.Init(choiceCounter, currentNode.choices[i].text);

                // Not all choices satisfy rules so need to keep track of choices that passed
                currentChoices.Add(currentNode.choices[i]);
                choiceCounter++;
            }
        }
    }

    public void OnChoiceSelected(DialogueChoice choice)
    {
        currentChoices.Clear();
        UiUtilMb.Instance.DestroyChildrenInContainer(choicesPanel);

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

