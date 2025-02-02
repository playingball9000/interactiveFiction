using System;
using System.Collections.Generic;

public class DialogueNode
{
    public string speaker;
    public string text;
    public string id;
    public List<DialogueChoice> choices = new();

    public DialogueNode(string id, string speaker, string text)
    {
        this.id = id;
        this.speaker = speaker;
        this.text = text;
    }

    public void AddChoice((string text, string dialogueNode) endChoice)
    {
        AddChoice(endChoice.text, endChoice.dialogueNode);
    }


    public void AddChoice(string text, string dialogueNode, Rule showRule = null, Action action = null)
    {
        choices.Add(new DialogueChoice { text = text, nextNode = dialogueNode, showRule = showRule, executeCode = action });
    }
}

public class DialogueChoice
{
    public string text;
    public string nextNode;
    public Rule showRule = null;
    public Action executeCode { get; set; }

    public void Execute()
    {
        executeCode?.Invoke();
    }
}
