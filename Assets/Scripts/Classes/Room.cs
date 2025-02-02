using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Room
{
    public string roomName;
    public string description;
    public string internalCode { get; set; }

    public List<Exit> exits = new();
    public List<NPC> npcs = new();
    public RoomItems roomItems = new();
    public List<IExaminable> roomScenery = new();

    public List<string> GetRoomInteractionDescriptions()
    {
        List<string> interactionDescriptionsInRoom = new();

        if (npcs.Any())
        {
            List<string> npcNames = npcs.Select(npc => npc.GetDisplayName()).ToList();
            interactionDescriptionsInRoom.Add("The following people are here: " + TmpTextTagger.Color(StringUtil.CreateCommaSeparatedString(npcNames), UiConstants.TEXT_COLOR_STORY_NPC));
        }
        if (roomItems.contents.Any())
        {
            interactionDescriptionsInRoom.Add(roomItems.ContentsToString());
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
        examinableThings.AddRange(roomItems.contents.ToList<IExaminable>());
        examinableThings.AddRange(roomScenery);

        examinableThings.AddRange(roomItems.contents.OfType<ContainerBase>()
                                    .Where(container => container.isAccessible())
                                    .SelectMany(openContainer => openContainer.contents)
                                    .ToList());

        return examinableThings;
    }

    public void DisplayRoomStoryText()
    {
        List<string> interactionDescriptionsInRoom = GetRoomInteractionDescriptions();

        string joinedInteractionDescriptions = TmpTextTagger.LineHeight("\n" + string.Join("\n", interactionDescriptionsInRoom.ToArray()), 160);

        string combinedText = TmpTextTagger.Bold(roomName) + "\n"
            + description
            + joinedInteractionDescriptions;

        StoryTextHandler.invokeUpdateStoryDisplay(combinedText);
    }

    public List<Fact> GetRoomFacts()
    {
        List<Fact> roomFacts = new();
        npcs.ForEach(npc => roomFacts.Add(new Fact { key = RuleConstants.KEY_IN_ROOM_NPC, value = npc.internalCode }));
        roomItems.contents.ForEach(item => roomFacts.Add(new Fact { key = RuleConstants.KEY_IN_ROOM_ITEM, value = item.referenceName }));

        return roomFacts;
    }

    public void RemoveItem(IItem item)
    {
        roomItems.RemoveItem(item);
    }

    public void AddItem(IItem item)
    {
        roomItems.AddItem(item);
    }

    public List<IItem> GetRoomItems()
    {
        return roomItems.contents;
    }

    public List<ContainerBase> GetRoomContainers()
    {
        return roomItems.contents.OfType<ContainerBase>().ToList();
    }

    public override string ToString()
    {
        string npcNames = StringUtil.CreateCommaSeparatedString(npcs.Select(npc => npc.referenceName).ToList());
        string itemNames = StringUtil.CreateCommaSeparatedString(roomItems.contents.Select(item => item.referenceName).ToList());
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
