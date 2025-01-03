using System.Collections.Generic;

public class DialogueNode
{
    public string id;
    public string speaker;
    public string text;
    public List<DialogueChoice> choices;
}

public class DialogueChoice
{
    public string text;
    public string nextId;
}
