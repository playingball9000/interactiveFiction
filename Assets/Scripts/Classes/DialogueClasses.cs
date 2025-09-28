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

    public DialogueNode AddChoice((string text, string dialogueNode) endChoice)
    {
        return AddChoice(endChoice.text, endChoice.dialogueNode);
    }


    public DialogueNode AddChoice(string text, string dialogueNode, Rule showRule = null, Action action = null)
    {
        choices.Add(new DialogueChoice { text = text, nextNodeId = dialogueNode, showRule = showRule, executeCode = action });
        return this;
    }
}

public class DialogueChoice
{
    public string text;
    public string nextNodeId;
    public Rule showRule = null;
    public Action executeCode { get; set; }

    public void Execute()
    {
        executeCode?.Invoke();
    }
}
