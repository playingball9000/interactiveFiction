using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour
{

    private DialogueNode currentNode;
    private Dictionary<string, DialogueNode> dialogues;

    List<string> dialogueLog = new List<string>();

    public TextMeshProUGUI UI_dialogueBox;
    public TextMeshProUGUI UI_dialogueChoicesBox;

    public Image speakerPortrait;

    KeyCode[] numpadKeys = { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
                         KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };


    public delegate void StartDialogueDelegate(NPC npc);
    public static StartDialogueDelegate invokeStartDialogue;

    private void OnEnable()
    {
        DialogueParser.invokeStartDialogue += StartDialogue;
    }

    private void OnDisable()
    {
        DialogueParser.invokeStartDialogue -= StartDialogue;
    }

    void Update()
    {
        // I guess I could also check if the dialogue window is active here...
        if (currentNode != null && Input.anyKeyDown)
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
            string logAsText = string.Join("\n", dialogueLog.ToArray());

            UI_dialogueBox.text = logAsText;
            //speakerPortrait.sprite = GetPortraitForSpeaker(node.speaker);

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

    private Sprite GetPortraitForSpeaker(string speaker)
    {
        // If I want to make portraits
        return null;
    }
}

