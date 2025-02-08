using System.Collections.Generic;
using System.Linq;

public class DialogueGraph
{
    private Dictionary<string, DialogueNode> nodes = new();
    public DialogueNode currentNode;

    public void AddNode(DialogueNode node)
    {
        if (!nodes.Any())
        {
            currentNode = node;
        }
        nodes.Add(node.id, node);
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
