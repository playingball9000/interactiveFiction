using System.Collections.Generic;

public class DialogueNode
{
    public string speaker;
    public string text;
    public List<DialogueChoice> choices = new();

    public void AddChoice(string text, DialogueNode dialogueNode, Rule showRule = null)
    {
        choices.Add(new DialogueChoice { text = text, nextNode = dialogueNode, showRule = showRule });
    }
}

public class DialogueChoice
{
    public string text;
    public DialogueNode nextNode;
    public Rule showRule = null;
}
