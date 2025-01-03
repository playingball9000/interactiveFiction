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

    public TextMeshProUGUI dialogueTextBox;
    public TextMeshProUGUI choicesTextBox;

    public Image speakerPortrait;

    void Awake()
    {
        LoadDialogue();
    }

    void Update()
    {
        if (currentNode != null && Input.anyKeyDown)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                // Check if the corresponding number key was pressed
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    DialogueChoice choice = currentNode.choices[i];
                    OnChoiceSelected(choice.text, choice.nextId);
                }
            }
        }
    }

    void LoadDialogue()
    {
        var jsonText = Resources.Load<TextAsset>("Dialogue/dialogue1").text;
        dialogues = JsonConvert.DeserializeObject<Dictionary<string, DialogueNode>>(jsonText);
    }

    public void DisplayDialogue(string dialogueId)
    {
        currentNode = dialogues.ContainsKey(dialogueId) ? dialogues[dialogueId] : null;
        if (currentNode != null)
        {
            string fullText = TmpTextTagger.Color($"{currentNode.speaker}   -   ", UiConstants.TEXT_COLOR_NPC_NAME) + TmpTextTagger.Color($"{currentNode.text}", UiConstants.TEXT_COLOR_NPC_TEXT);
            dialogueLog.Add(fullText + "\n");
            string logAsText = string.Join("\n", dialogueLog.ToArray());

            dialogueTextBox.text = logAsText;
            //speakerPortrait.sprite = GetPortraitForSpeaker(node.speaker);

            choicesTextBox.text = "";
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                choicesTextBox.text += $"{i + 1}. {currentNode.choices[i].text}\n";
            }
        }
        else
        {
            Debug.LogWarning($"Dialogue with ID '{dialogueId}' not found.");
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

