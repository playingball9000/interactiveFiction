using UnityEditor;

public static class DialogueBuilder
{
    //TODO: probably make this better, caching, set on npc?
    public static DialogueGraph BuildDialogue(string dialogueFile)
    {
        if (dialogueFile == "womanDialogue")
        {
            return BuildWomanDialogue();
        }

        return null;
    }

    public static (string, string) EndConversation(string text = "")
    {
        return (text + " [End Conversation]", null);
    }

    public static DialogueGraph BuildWomanDialogue()
    {
        DialogueGraph dg = new();

        dg.AddNode(new("node1", "Woman", "The Woman looks at you and smiles."));
        dg.GetNode("node1").AddChoice("I think they're relaxing. What about you?", "node11");
        dg.GetNode("node1").AddChoice("Honestly, they can be a bit boring sometimes.", "node12");
        dg.GetNode("node1").AddChoice("What did you think of this book?", "node13",
           new Rule().AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, RuleKey.InInventory, "item_old_book"))),
           () => { StoryTextHandler.invokeUpdateStoryDisplay("D'OH"); });
        dg.GetNode("node1").AddChoice(EndConversation());

        dg.AddNode(new("node11", "Woman", "They nod, leaning back. 'Yeah, they're kind of soothing, aren't they? Just the sound of the tracks and the scenery flying by.'"));
        dg.AddNode(new("node12", "Woman", "They chuckle. 'Fair enough. I guess not everyone finds them exciting. What do you do to pass the time?'"));
        dg.AddNode(new("node13", "Woman", "They shrug. 'I didn't care for it.'"));
        dg.GetNode("node11").AddChoice("Exactly! It's a nice way to clear your head.", "node111");
        dg.GetNode("node11").AddChoice("Sometimes I lose track of time staring out the window.", "node112");
        dg.GetNode("node12").AddChoice("I usually bring a book or listen to music.", "node121");
        dg.GetNode("node12").AddChoice("Mostly just stare out the window. It's kind of mesmerizing.", "node122");
        dg.GetNode("node13").AddChoice(EndConversation("Oh LOL"));

        dg.AddNode(new("node111", "Woman", "They smile warmly. 'Yeah, it's like a little escape, isn't it? I don't even mind when it's a long trip.'"));
        dg.AddNode(new("node112", "Woman", "They laugh softly. 'Right? It's like the world becomes a giant movie screen.'"));
        dg.AddNode(new("node121", "Woman", "They nod thoughtfully. 'Good choices. Music and books definitely help the time fly.'"));
        dg.AddNode(new("node122", "Woman", "They grin. 'Same here. There's something so calming about watching the world blur by.'"));
        dg.GetNode("node111").AddChoice(EndConversation("Exactly. It's like time slows down a bit."));
        dg.GetNode("node112").AddChoice(EndConversation());
        dg.GetNode("node121").AddChoice("Do you have a favorite book or playlist for train rides?", "node1211");
        dg.GetNode("node122").AddChoice("Especially when the light hits just right.", "node1222");

        dg.AddNode(new("node1211", "Woman", "They light up. 'Oh, absolutely. For books, I like thrillers—makes the calm of the train feel like the perfect backdrop.'"));
        dg.AddNode(new("node1222", "Woman", "They glance out the window, their tone reflective. 'Yeah, especially during sunrise or sunset. It's like nature showing off.'"));
        dg.GetNode("node1211").AddChoice(EndConversation("That's an interesting contrast. Thrills in a peaceful setting."));
        dg.GetNode("node1222").AddChoice(EndConversation("It's moments like that that make me love train rides."));

        return dg;
    }

    public static DialogueGraph MaryDialogue1()
    {
        DialogueGraph dg = new();
        dg.AddNode(new("node1", "Mary", "Today is your big day."));

        return dg;
    }
}