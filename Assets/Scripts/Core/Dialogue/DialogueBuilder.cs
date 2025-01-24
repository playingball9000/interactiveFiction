public static class DialogueBuilder
{
    //TODO: probably make this better, caching, set on npc?
    public static DialogueNode BuildDialogue(string dialogueFile)
    {
        if (dialogueFile == "womanDialogue")
        {
            return BuildWomanDialogue();
        }

        return null;
    }

    public static DialogueNode BuildWomanDialogue()
    {
        DialogueNode node1 = new()
        {
            speaker = "Passenger",
            text = "The passenger looks out the window and smiles. 'So, what do you think about train rides?'",
        };

        DialogueNode node11 = new()
        {
            speaker = "Passenger",
            text = "They nod, leaning back. 'Yeah, they're kind of soothing, aren't they? Just the sound of the tracks and the scenery flying by.'",
        };

        DialogueNode node12 = new()
        {
            speaker = "Passenger",
            text = "They chuckle. 'Fair enough. I guess not everyone finds them exciting. What do you do to pass the time?'",
        };

        DialogueNode node13 = new()
        {
            speaker = "Passenger",
            text = "They shrug. 'I didn't care for it.'",
        };

        DialogueNode node111 = new()
        {
            speaker = "Passenger",
            text = "They smile warmly. 'Yeah, it's like a little escape, isn't it? I don't even mind when it's a long trip.'",
        };

        DialogueNode node112 = new()
        {
            speaker = "Passenger",
            text = "They laugh softly. 'Right? It's like the world becomes a giant movie screen.'",
        };

        DialogueNode node121 = new()
        {
            speaker = "Passenger",
            text = "They nod thoughtfully. 'Good choices. Music and books definitely help the time fly.'",
        };

        DialogueNode node122 = new()
        {
            speaker = "Passenger",
            text = "They grin. 'Same here. There's something so calming about watching the world blur by.'",
        };

        DialogueNode node1211 = new()
        {
            speaker = "Passenger",
            text = "They light up. 'Oh, absolutely. For books, I like thrillers—makes the calm of the train feel like the perfect backdrop.'",
        };

        DialogueNode node1222 = new()
        {
            speaker = "Passenger",
            text = "They glance out the window, their tone reflective. 'Yeah, especially during sunrise or sunset. It's like nature showing off.'",
        };

        // Link choices to nodes
        node1.AddChoice("I think they're relaxing. What about you?", node11);
        node1.AddChoice("Honestly, they can be a bit boring sometimes.", node12);
        node1.AddChoice(
            "What did you think of this book?",
            node13,
            new Rule().AddCriteria(facts => Criterion.FactValueEquals(facts, "in_inventory", "book")));
        node1.AddChoice("[End Conversation]", null);

        node13.AddChoice("Oh LOL", null);
        node11.AddChoice("Exactly! It's a nice way to clear your head.", node111);
        node11.AddChoice("Sometimes I lose track of time staring out the window.", node112);

        node12.AddChoice("I usually bring a book or listen to music.", node121);
        node12.AddChoice("Mostly just stare out the window. It's kind of mesmerizing.", node122);
        node111.AddChoice("Exactly. It's like time slows down a bit.", null);
        node112.AddChoice("[End Conversation]", null);

        node121.AddChoice("Do you have a favorite book or playlist for train rides?", node1211);
        node122.AddChoice("Especially when the light hits just right.", node1222);
        node1211.AddChoice("That's an interesting contrast. Thrills in a peaceful setting.", null);
        node1222.AddChoice("It's moments like that that make me love train rides.", null);

        return node1;
    }
}