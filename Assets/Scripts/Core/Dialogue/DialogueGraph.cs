using System.Collections.Generic;
using System.Linq;

public class DialogueGraph
{
    private Dictionary<string, DialogueNode> nodes = new();
    public DialogueNode currentNode;

    public DialogueNode AddNode(string id, string speaker, string text)
    {
        DialogueNode newNode = new(id, speaker, text);
        if (!nodes.Any())
        {
            currentNode = newNode;
        }
        nodes.Add(newNode.id, newNode);
        return newNode;
    }

    public DialogueNode GetNode(string id)
    {
        return nodes.TryGetValue(id, out DialogueNode dialogueNode) ? dialogueNode : null;
    }

    public void SetCurrentNode(string id)
    {
        currentNode = nodes[id];
    }
}
