using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Room
{
    public string description;
    public string roomName;

    public List<Exit> exits = new();
    public List<NPC> npcs = new();
    public List<IItem> roomItems = new();
    public List<IExaminable> roomScenery = new();

    public List<string> GetRoomInteractionDescriptions()
    {
        List<string> interactionDescriptionsInRoom = new();


        if (npcs?.Any() == true)
        {
            List<string> npcNames = npcs.Select(npc => npc.referenceName).ToList();
            interactionDescriptionsInRoom.Add("The following people are here: " + StringUtil.CreateCommaSeparatedString(npcNames) + "\n");
        }

        if (roomItems?.Any() == true)
        {
            List<string> itemNames = roomItems.Select(item => item.referenceName).ToList();
            interactionDescriptionsInRoom.Add("Items in room: " + StringUtil.CreateCommaSeparatedString(itemNames) + "\n");

        }
        foreach (Exit exit in exits)
        {
            interactionDescriptionsInRoom.Add(exit.exitDescription);
        }
        return interactionDescriptionsInRoom;
    }

    public List<IExaminable> GetExaminableThings()
    {
        List<IExaminable> examinableThings = npcs.ToList<IExaminable>();
        examinableThings.AddRange(roomItems.ToList<IExaminable>());
        examinableThings.AddRange(roomScenery);

        examinableThings.AddRange(roomItems.OfType<IContainer>()
                                    .Where(container => container.isOpen)
                                    .SelectMany(openContainer => openContainer.contents)
                                    .ToList());

        return examinableThings;
    }

    public void DisplayRoomStoryText()
    {
        List<string> interactionDescriptionsInRoom = GetRoomInteractionDescriptions();

        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());

        string combinedText = TmpTextTagger.Bold(roomName) + "\n"
            + description + "\n\n"
            + joinedInteractionDescriptions;

        StoryTextHandler.invokeUpdateStoryDisplay(combinedText);
    }

    public List<Fact> GetRoomFacts()
    {
        List<Fact> roomFacts = new();
        npcs.ForEach(npc => roomFacts.Add(new Fact { key = "in_room_npc", value = npc.referenceName }));
        roomItems.ForEach(item => roomFacts.Add(new Fact { key = "in_room_item", value = item.referenceName }));

        return roomFacts;
    }

    public void RemoveItem(IItem item)
    {
        if (roomItems.Contains(item))
        {
            roomItems.Remove(item);
        }
    }

    public void AddItem(IItem item)
    {
        roomItems.Add(item);
    }

    public override string ToString()
    {
        string npcNames = StringUtil.CreateCommaSeparatedString(npcs.Select(npc => npc.referenceName).ToList());
        string itemNames = StringUtil.CreateCommaSeparatedString(roomItems.Select(item => item.referenceName).ToList());
        string exitsPaths = StringUtil.CreateCommaSeparatedString(exits.Select(ex => ex.ToString()).ToList());

        string toString = $@"
            roomName: {roomName}
            description: {description}
            npcNames: {npcNames}
            itemNames: {itemNames}
            exitsPaths: {exitsPaths}
        ";

        return toString;
    }
}
