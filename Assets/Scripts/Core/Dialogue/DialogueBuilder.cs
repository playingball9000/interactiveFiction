
public static class DialogueBuilder
{
    //TODO: probably make this better, caching, set on npc?
    public static DialogueGraph BuildDialogue(string dialogueFile)
    {

        switch (dialogueFile)
        {
            case "womanDialogue":
                return BuildWomanDialogue();
            case "maryDialogue":
                return MaryDialogue1();
            default:
                break;
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

        dg.AddNode("node1", "Woman", "The Woman looks at you and smiles.")
            .AddChoice("I think they're relaxing. What about you?", "node11")
            .AddChoice("Honestly, they can be a bit boring sometimes.", "node12")
            .AddChoice("What did you think of this book?", "node13",
                new Rule().AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, RuleKey.InInventory, "item_old_book"))),
                () => { StoryTextHandler.invokeUpdateStoryDisplay("D'OH"); })
            .AddChoice(EndConversation());

        dg.AddNode("node11", "Woman", "They nod, leaning back. 'Yeah, they're kind of soothing, aren't they? Just the sound of the tracks and the scenery flying by.'");
        dg.AddNode("node12", "Woman", "They chuckle. 'Fair enough. I guess not everyone finds them exciting. What do you do to pass the time?'");
        dg.AddNode("node13", "Woman", "They shrug. 'I didn't care for it.'");
        dg.GetNode("node11").AddChoice("Exactly! It's a nice way to clear your head.", "node111");
        dg.GetNode("node11").AddChoice("Sometimes I lose track of time staring out the window.", "node112");
        dg.GetNode("node12").AddChoice("I usually bring a book or listen to music.", "node121");
        dg.GetNode("node12").AddChoice("Mostly just stare out the window. It's kind of mesmerizing.", "node122");
        dg.GetNode("node13").AddChoice(EndConversation("Oh LOL"));

        dg.AddNode("node111", "Woman", "They smile warmly. 'Yeah, it's like a little escape, isn't it? I don't even mind when it's a long trip.'");
        dg.AddNode("node112", "Woman", "They laugh softly. 'Right? It's like the world becomes a giant movie screen.'");
        dg.AddNode("node121", "Woman", "They nod thoughtfully. 'Good choices. Music and books definitely help the time fly.'");
        dg.AddNode("node122", "Woman", "They grin. 'Same here. There's something so calming about watching the world blur by.'");
        dg.GetNode("node111").AddChoice(EndConversation("Exactly. It's like time slows down a bit."));
        dg.GetNode("node112").AddChoice(EndConversation());
        dg.GetNode("node121").AddChoice("Do you have a favorite book or playlist for train rides?", "node1211");
        dg.GetNode("node122").AddChoice("Especially when the light hits just right.", "node1222");

        dg.AddNode("node1211", "Woman", "They light up. 'Oh, absolutely. For books, I like thrillers—makes the calm of the train feel like the perfect backdrop.'");
        dg.AddNode("node1222", "Woman", "They glance out the window, their tone reflective. 'Yeah, especially during sunrise or sunset. It's like nature showing off.'");
        dg.GetNode("node1211").AddChoice(EndConversation("That's an interesting contrast. Thrills in a peaceful setting."));
        dg.GetNode("node1222").AddChoice(EndConversation("It's moments like that that make me love train rides."));

        return dg;
    }

    public static DialogueGraph MaryDialogue1()
    {
        string speaker = "Mary";
        DialogueGraph dg = new();
        dg.AddNode("node1", speaker, @"The woman is somehow both imposing and comforting. Before you knew her as [The Hearth], she took care of you and the other kids at the orphanage. Today she is overseeing what is likely the most important day of your life so far. ""It's your big day. How are you feeling?""")
            .AddChoice("Truth: I'm nervous.", "node11")
            .AddChoice("Lie: I'm nervous.", "node12")
            .AddChoice("Truth: I'm ready for anything!", "node13")
            .AddChoice("Lie: I'm ready for anything!", "node14");

        dg.AddNode("node11", speaker, @"""You're ready! Even if you don't feel it. There's no harm in taking things slow.""").AddChoice("[More]", "node1x1");

        dg.AddNode("node12", speaker, @"Her eyes linger on you. ""Are you really? No need to be humble."" She chuckles lightly.").AddChoice("[More]", "node1x1");

        dg.AddNode("node13", speaker, @"""Indeed, you are! And that's enough. You will overcome whatever comes your way.""").AddChoice("[More]", "node1x1");

        dg.AddNode("node14", speaker, @"A slight grin forms on her face. ""You know, I believe in you. I'm not worried about you at all.""").AddChoice("[More]", "node1x1");

        dg.AddNode("node1x1", speaker, @"She puts both hands on your shoulders. ""No matter what anyone says, you are courageous and strong. You'll go far in the deep!""")
            .AddChoice("Thanks, Mary. I had some other questions...", "nodeMainQuestion")
            .AddChoice(EndConversation("Thank you, Mary. I'll go for it!"));

        dg.AddNode("nodeMainQuestion", speaker, @"Mary regards you warmly.")
            .AddChoice("How are *you* feeling?", "nodeMainQuestion1")
            .AddChoice("Can you tell me about the test?", "nodeMainQuestion2")
            .AddChoice("What was your graduation day like?", "nodeMainQuestion3")
            .AddChoice(EndConversation("I'll go for it!"));

        dg.AddNode("nodeMainQuestion1", speaker, @"A hint of sadness creeps into her eyes for just a moment before fading into her gentle smile. ""Don't spare a thought for me. I'm so proud of each one of you for coming so far.""")
            .AddChoice("Can you tell me about the test?", "nodeMainQuestion2")
            .AddChoice("What was your graduation day like?", "nodeMainQuestion3")
            .AddChoice(EndConversation("I'll go for it!"));

        dg.AddNode("nodeMainQuestion2", speaker, @"You watch as she thinks about it for a moment, she wants to tell you something. ""I'm sorry, it wouldn't be fair to the others.""")
        .AddChoice("How are *you* feeling?", "nodeMainQuestion1")
            .AddChoice("What was your graduation day like?", "nodeMainQuestion3")
            .AddChoice(EndConversation("I'll go for it!"));

        dg.AddNode("nodeMainQuestion3", speaker, @"""We didn't..."" She takes small breath. ""When I first started, those who survived the deep became Trail Blazers. You would have passed."" She winks at you.")
        .AddChoice("How are *you* feeling?", "nodeMainQuestion1")
            .AddChoice("Can you tell me about the test?", "nodeMainQuestion2")
            .AddChoice(EndConversation("I'll go for it!"));

        /*

        Mary hums a familiar tune.
        */
        return dg;
    }
}